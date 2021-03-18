using Microsoft.Data.SqlClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.Data.Helpers;
using TFW.Framework.Data.Options;
using TFW.Framework.Data.Wrappers;

namespace TFW.Framework.Data
{
    /// <summary>
    /// NOTE: Microsoft SqlConnection already has pooling mechanism
    /// </summary>
    public class SqlConnectionPoolManager : IDbConnectionPoolManager
    {
        public event RetryAddToPoolErrorEventHandler RetryAddToPoolError;

        private readonly ConcurrentDictionary<string, ConcurrentQueue<SqlPooledDbConnectionWrapper>> _pools;
        private readonly ConcurrentDictionary<Guid, (SqlPooledDbConnectionWrapper, DateTime)> _connCreatedTimes;
        private readonly ConcurrentDictionary<string, SqlConnectionPoolOptions> _poolOptions;
        private readonly object _poolsLock;
        private bool disposedValue;
        private Thread _watcherThread;
        private readonly SqlConnectionPoolManagerOptions _options;

        public SqlConnectionPoolManager(SqlConnectionPoolManagerOptions options)
        {
            _pools = new ConcurrentDictionary<string, ConcurrentQueue<SqlPooledDbConnectionWrapper>>();
            _connCreatedTimes = new ConcurrentDictionary<Guid, (SqlPooledDbConnectionWrapper, DateTime)>();
            _poolOptions = new ConcurrentDictionary<string, SqlConnectionPoolOptions>();
            _poolsLock = new object();
            _options = options;
            SqlConnection.ClearAllPools();
        }

        public SqlConnectionPoolManager() : this(new SqlConnectionPoolManagerOptions())
        {
        }

        public Task<DbConnection> GetDbConnectionAsync(string poolKey, bool createNonPooledConnIfExceedLimit = false)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(SqlConnectionPoolManager));

            if (!_poolOptions.ContainsKey(poolKey)) throw new KeyNotFoundException();

            var connections = _pools[poolKey];
            SqlPooledDbConnectionWrapper availableConn = null;

            do
            {
                if (!connections.TryDequeue(out availableConn) && !SetupNewConnection(poolKey))
                    break;
            }
            while (availableConn?.DbConnection.State != ConnectionState.Open);

            DbConnection conn = availableConn?.DbConnection;

            if (availableConn?.DbConnection.State != ConnectionState.Open && createNonPooledConnIfExceedLimit)
                conn = new SqlConnection(_poolOptions[poolKey].ConnectionString);

            return Task.FromResult(conn);
        }

        public async Task InitDbConnectionAsync(SqlConnectionPoolOptions options, string poolKey = null)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(SqlConnectionPoolManager));

            poolKey = poolKey ?? Guid.NewGuid().ToString();

            #region Validation
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.LifetimeInMinutes <= 0)
                throw new ArgumentException(nameof(options.LifetimeInMinutes));

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
                throw new ArgumentException(nameof(options.ConnectionString));

            if (options.MaximumConnections < 1)
                throw new ArgumentException(nameof(options.MaximumConnections));

            if (options.MinimumConnections < 0 || options.MinimumConnections > options.MaximumConnections)
                throw new ArgumentException(nameof(options.MinimumConnections));

            if (options.MaximumRetryWhenFailure < 0)
                throw new ArgumentException(nameof(options.MaximumRetryWhenFailure));

            if (_poolOptions.ContainsKey(poolKey))
                throw new InvalidOperationException($"Already initialized '{poolKey}'");
            #endregion

            try
            {
                _poolOptions[poolKey] = options.DeepClone();
                _pools[poolKey] = new ConcurrentQueue<SqlPooledDbConnectionWrapper>();

                for (var i = 0; i < options.MinimumConnections; i++)
                    SetupNewConnection(poolKey);

                // Create watcher
                if (_watcherThread == null)
                {
                    _watcherThread = new Thread(new ThreadStart(async () => await Watch()))
                    {
                        IsBackground = true
                    };

                    _watcherThread.Start();
                }
            }
            catch (Exception e)
            {
                await ReleasePoolAsync(poolKey);
                throw e;
            }
        }

        public async Task ReleasePoolAsync(string poolKey)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(SqlConnectionPoolManager));

            _poolOptions.Remove(poolKey, out _);

            var tasks = new List<Task>();

            foreach (var wrapper in _pools[poolKey])
            {
                tasks.Add(wrapper.DbConnection.DisposeAsync().AsTask());
                _connCreatedTimes.Remove(wrapper.DbConnection.ClientConnectionId, out _);
            }

            await Task.WhenAll(tasks);

            _pools[poolKey].Clear();
            _pools.Remove(poolKey, out _);
        }

        public async Task ReleaseAllPoolsAsync()
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(SqlConnectionPoolManager));

            var tasks = new List<Task>();

            foreach (var key in _poolOptions.Keys)
                tasks.Add(ReleasePoolAsync(key));

            await Task.WhenAll(tasks);
        }

        private bool SetupNewConnection(string poolKey)
        {
            if (disposedValue) return false;

            lock (_poolsLock)
            {
                if (_pools[poolKey].Count >= _poolOptions[poolKey].MaximumConnections)
                    return false;

                var dbConn = new SqlConnection(_poolOptions[poolKey].ConnectionString);
                var wrapper = new SqlPooledDbConnectionWrapper(dbConn, poolKey);

                dbConn.StateChange += async (sender, e) => await DbConn_StateChange(sender, e, wrapper);

                wrapper.DbConnection.Open();

                _pools[wrapper.PoolKey].Enqueue(wrapper);
                _connCreatedTimes[wrapper.DbConnection.ClientConnectionId] = (wrapper, DateTime.UtcNow);

                return true;
            }
        }

        private async Task DbConn_StateChange(object sender, StateChangeEventArgs e, SqlPooledDbConnectionWrapper wrapper)
        {
            var connectionState = e.CurrentState;

            if (disposedValue || (connectionState != ConnectionState.Closed
                && connectionState != ConnectionState.Broken)
                || HasExceededLifetime(wrapper)) return;

            await RetryAddToPoolAsync(wrapper);
        }

        public Task RetryAddToPoolAsync(DbConnection connection)
        {
            if (connection is SqlConnection == false) throw new InvalidOperationException("Unsupported connection");

            var wrapper = _connCreatedTimes[(connection as SqlConnection).ClientConnectionId].Item1;

            return RetryAddToPoolAsync(wrapper);
        }

        private Task RetryAddToPoolAsync(SqlPooledDbConnectionWrapper wrapper)
        {
            if (disposedValue) return Task.CompletedTask;

            return Task.Run(() =>
            {
                for (var i = 0; i < _poolOptions[wrapper.PoolKey].MaximumRetryWhenFailure; i++)
                {
                    try
                    {
                        lock (_poolsLock)
                        {
                            if (HasExceededLifetime(wrapper)) return;

                            if (_pools[wrapper.PoolKey].Count < _poolOptions[wrapper.PoolKey].MaximumConnections)
                            {
                                wrapper.DbConnection.Open();

                                _pools[wrapper.PoolKey].Enqueue(wrapper);
                                _connCreatedTimes[wrapper.DbConnection.ClientConnectionId] = (wrapper, DateTime.UtcNow);

                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RetryAddToPoolError?.Invoke(ex, i + 1);
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(_options.RetryIntervalInSeconds));
                }
            });
        }

        private bool HasExceededLifetime(SqlPooledDbConnectionWrapper wrapper)
        {
            var createdTime = _connCreatedTimes[wrapper.DbConnection.ClientConnectionId].Item2;

            var lifeTime = (DateTime.UtcNow - createdTime).TotalMinutes;

            return (lifeTime >= _poolOptions[wrapper.PoolKey].LifetimeInMinutes);
        }

        private async Task Watch()
        {
            Thread.Sleep(TimeSpan.FromMinutes(_options.WatchIntervalInMinutes));

            var tasks = new List<Task>();

            foreach (var key in _poolOptions.Keys)
            {
                var pool = _pools[key];
                lock (_poolsLock)
                {
                    SqlPooledDbConnectionWrapper currentConn, firstConn = null;
                    if (pool.TryDequeue(out currentConn))
                    {
                        firstConn = currentConn;

                        do
                        {
                            if (HasExceededLifetime(currentConn) && !currentConn.DbConnection.IsOpening())
                            {
                                tasks.Add(currentConn.DbConnection.DisposeAsync().AsTask());
                                _connCreatedTimes.Remove(currentConn.DbConnection.ClientConnectionId, out _);
                            }
                            else pool.Enqueue(currentConn);

                            if (!pool.TryDequeue(out currentConn)) break;
                        }
                        while (currentConn != firstConn);
                    }
                }
            }

            await Task.WhenAll(tasks);
        }

        #region Dispose
        public ValueTask DisposeAsync()
        {
            return new ValueTask(DisposeAsync(true));
        }

        protected virtual async Task DisposeAsync(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    await ReleaseAllPoolsAsync();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~SqlConnectionPool()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            DisposeAsync(disposing: true).Wait();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
