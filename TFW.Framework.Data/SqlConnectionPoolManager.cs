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
using TFW.Framework.Data.Models;
using TFW.Framework.Data.Options;
using TFW.Framework.Data.Wrappers;

namespace TFW.Framework.Data
{
    /// <summary>
    /// NOTE: Microsoft SqlConnection already has pooling mechanism
    /// </summary>
    public class SqlConnectionPoolManager : IDbConnectionPoolManager
    {
        public bool IsNullObject => false;

        public event TryReturnToPoolErrorEventHandler TryReturnToPoolError;
        public event WatcherThreadErrorEventHandler WatcherThreadError;
        public event NewConnectionErrorEventHandler NewConnectionError;

        private readonly ConcurrentDictionary<string, ConcurrentQueue<PooledDbConnectionWrapper>> _pools;
        private readonly ConcurrentDictionary<Guid, ConnectionInfo> _connInfos;
        private readonly ConcurrentDictionary<string, ConnectionPoolOptions> _poolOptions;
        private readonly object _poolsLock;
        private bool disposedValue;
        private Thread _watcherThread;
        private readonly SqlConnectionPoolManagerOptions _options;

        public SqlConnectionPoolManager(SqlConnectionPoolManagerOptions options)
        {
            _pools = new ConcurrentDictionary<string, ConcurrentQueue<PooledDbConnectionWrapper>>();
            _connInfos = new ConcurrentDictionary<Guid, ConnectionInfo>();
            _poolOptions = new ConcurrentDictionary<string, ConnectionPoolOptions>();
            _poolsLock = new object();
            _options = options;
        }

        public SqlConnectionPoolManager() : this(new SqlConnectionPoolManagerOptions())
        {
        }

        public Task<DbConnection> GetDbConnectionAsync(string poolKey)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(SqlConnectionPoolManager));

            if (!_poolOptions.ContainsKey(poolKey)) throw new KeyNotFoundException();

            var connections = _pools[poolKey];
            PooledDbConnectionWrapper availableConn = null;

            do
            {
                if (!connections.TryDequeue(out availableConn) && !SetupNewConnection(poolKey))
                    break;
            }
            while (availableConn?.DbConnection.State != ConnectionState.Open);

            return Task.FromResult(availableConn?.DbConnection);
        }

        public async Task InitDbConnectionAsync(ConnectionPoolOptions options, string poolKey = null)
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
                _pools[poolKey] = new ConcurrentQueue<PooledDbConnectionWrapper>();

                for (var i = 0; i < options.MinimumConnections; i++)
                    SetupNewConnection(poolKey);

                // Create watcher
                if (_watcherThread == null)
                {
                    _watcherThread = new Thread(new ThreadStart(Watch))
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
                _connInfos.Remove((wrapper.DbConnection as SqlConnection).ClientConnectionId, out _);
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

                try
                {
                    var dbConn = new SqlConnection(_poolOptions[poolKey].ConnectionString);
                    var wrapper = new PooledDbConnectionWrapper(dbConn, poolKey);

                    dbConn.StateChange += async (sender, e) => await DbConn_StateChange(sender, e, wrapper);

                    wrapper.DbConnection.Open();

                    _pools[wrapper.PoolKey].Enqueue(wrapper);
                    _connInfos[(wrapper.DbConnection as SqlConnection).ClientConnectionId] = new ConnectionInfo()
                    {
                        CreatedTime = DateTime.UtcNow,
                        Wrapper = wrapper
                    };
                }
                catch (Exception ex)
                {
                    NewConnectionError?.Invoke(ex, poolKey);
                    throw ex;
                }

                return true;
            }
        }

        private async Task DbConn_StateChange(object sender, StateChangeEventArgs e, PooledDbConnectionWrapper wrapper)
        {
            var connectionState = e.CurrentState;

            if (disposedValue || (connectionState != ConnectionState.Closed
                && connectionState != ConnectionState.Broken)
                || HasExceededLifetime(wrapper)) return;

            await TryReturnToPoolAsync(wrapper);
        }

        public Task TryReturnToPoolAsync(DbConnection connection)
        {
            if (connection is SqlConnection == false) throw new InvalidOperationException("Unsupported connection");

            var sqlConn = connection as SqlConnection;

            if (!_connInfos.ContainsKey(sqlConn.ClientConnectionId)) return Task.CompletedTask;

            var wrapper = _connInfos[sqlConn.ClientConnectionId].Wrapper;

            return TryReturnToPoolAsync(wrapper);
        }

        private Task TryReturnToPoolAsync(PooledDbConnectionWrapper wrapper)
        {
            if (disposedValue) return Task.CompletedTask;

            return Task.Run(() =>
            {
                var poolOption = _poolOptions[wrapper.PoolKey];

                TryAction((retryCount) =>
                {
                    try
                    {
                        lock (_poolsLock)
                        {
                            if (HasExceededLifetime(wrapper)) return true;

                            if (_pools[wrapper.PoolKey].Count < poolOption.MaximumConnections)
                            {
                                if (!wrapper.DbConnection.IsOpening())
                                    wrapper.DbConnection.Open();

                                _pools[wrapper.PoolKey].Enqueue(wrapper);
                                _connInfos[(wrapper.DbConnection as SqlConnection).ClientConnectionId] = new ConnectionInfo
                                {
                                    Wrapper = wrapper,
                                    CreatedTime = DateTime.UtcNow
                                };
                            }

                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        TryReturnToPoolError?.Invoke(ex, retryCount);
                        return false;
                    }
                }, poolOption);
            });
        }

        private bool HasExceededLifetime(PooledDbConnectionWrapper wrapper)
        {
            var createdTime = _connInfos[(wrapper.DbConnection as SqlConnection).ClientConnectionId].CreatedTime;

            var lifeTime = (DateTime.UtcNow - createdTime).TotalMinutes;

            return (lifeTime >= _poolOptions[wrapper.PoolKey].LifetimeInMinutes);
        }

        private void Watch()
        {
            Thread.Sleep(TimeSpan.FromMinutes(_options.WatchIntervalInMinutes));

            foreach (var poolKey in _poolOptions.Keys)
            {
                var pool = _pools[poolKey];
                var poolOption = _poolOptions[poolKey];
                PooledDbConnectionWrapper currentConn, firstConn = null;
                var disposes = new List<DbConnection>();
                var opens = new List<PooledDbConnectionWrapper>();

                if (pool.TryDequeue(out currentConn))
                {
                    firstConn = currentConn;

                    lock (_poolsLock)
                    {
                        do
                        {
                            if (HasExceededLifetime(currentConn) && !currentConn.DbConnection.IsOpening())
                            {
                                if (pool.Count > poolOption.MinimumConnections)
                                {
                                    disposes.Add(currentConn.DbConnection);
                                    _connInfos.Remove((currentConn.DbConnection as SqlConnection).ClientConnectionId, out _);
                                }
                                else
                                {
                                    opens.Add(currentConn);
                                    _connInfos[(currentConn.DbConnection as SqlConnection).ClientConnectionId] = new ConnectionInfo
                                    {
                                        Wrapper = currentConn,
                                        CreatedTime = DateTime.UtcNow
                                    };
                                }
                            }
                            else pool.Enqueue(currentConn);

                            if (!pool.TryDequeue(out currentConn)) break;
                        }
                        while (currentConn != firstConn);
                    }
                }

                foreach (var needDisposeConn in disposes)
                {
                    try
                    {
                        needDisposeConn.Dispose();
                    }
                    catch (Exception ex)
                    {
                        WatcherThreadError?.Invoke(ex, currentConn.DbConnection);
                    }
                }

                foreach (var needOpenConn in opens)
                {
                    var success = TryAction((retryCount) =>
                    {
                        try
                        {
                            lock (_poolsLock)
                            {
                                if (pool.Count < poolOption.MaximumConnections)
                                {
                                    needOpenConn.DbConnection.Open();
                                    pool.Enqueue(needOpenConn);
                                }
                            }
                            return true;
                        }
                        catch (Exception ex)
                        {
                            WatcherThreadError?.Invoke(ex, currentConn.DbConnection);
                            return false;
                        }
                    }, poolOption);

                    if (!success)
                        _connInfos.Remove((currentConn.DbConnection as SqlConnection).ClientConnectionId, out _);
                }

                while (pool.Count < poolOption.MinimumConnections)
                    TryAction((retryCount) =>
                    {
                        try
                        {
                            SetupNewConnection(poolKey);
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }, poolOption);
            }
        }

        private bool TryAction(Func<int, bool> action, ConnectionPoolOptions options)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            for (var i = 0; i < options.MaximumRetryWhenFailure; i++)
            {
                // 'action' must handle exceptions explicitly
                var success = action(i + 1);

                if (success) return true;

                Thread.Sleep(TimeSpan.FromSeconds(options.RetryIntervalInSeconds));
            }

            return false;
        }

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
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
