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
    /// This is for learning purpose only
    /// </summary>
    public class SqlConnectionPoolManager : IDbConnectionPoolManager
    {
        public bool IsNullObject => false;

        public event TryReturnToPoolErrorEventHandler TryReturnToPoolError;
        public event WatcherThreadErrorEventHandler WatcherThreadError;
        public event NewConnectionErrorEventHandler NewConnectionError;
        public event ReleaseConnectionErrorEventHandler ReleaseConnectionError;

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

            var pool = _pools[poolKey];
            PooledDbConnectionWrapper availableConn = null;

            do
            {
                var hasConn = TryGetConnection(pool, out availableConn, out _);

                if (!hasConn && !SetupNewConnection(poolKey))
                    break;
            }
            while (availableConn?.DbConnection.State != ConnectionState.Open);

            return Task.FromResult(availableConn?.DbConnection);
        }

        public async Task<string> InitDbConnectionAsync(ConnectionPoolOptions options)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(SqlConnectionPoolManager));

            var poolKey = GetPoolKeyFromConnStr(options.ConnectionString);

            #region Validation
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.LifetimeInMinutes <= 0)
                throw new ArgumentException(nameof(options.LifetimeInMinutes));

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
                throw new ArgumentException(nameof(options.ConnectionString));

            if (options.MaxPoolSize < 1)
                throw new ArgumentException(nameof(options.MaxPoolSize));

            if (options.MinPoolSize < 0 || options.MinPoolSize > options.MaxPoolSize)
                throw new ArgumentException(nameof(options.MinPoolSize));

            if (options.MaximumRetryWhenFailure < 0)
                throw new ArgumentException(nameof(options.MaximumRetryWhenFailure));

            if (_poolOptions.ContainsKey(poolKey))
                throw new InvalidOperationException($"Already initialized '{poolKey}'");
            #endregion

            try
            {
                _poolOptions[poolKey] = options.Snapshot();
                _pools[poolKey] = new ConcurrentQueue<PooledDbConnectionWrapper>();

                for (var i = 0; i < options.MinPoolSize; i++)
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

            return poolKey;
        }

        public Task ReleasePoolAsync(string poolKey)
        {
            if (disposedValue) throw new ObjectDisposedException(nameof(SqlConnectionPoolManager));

            lock (_poolsLock)
            {
                var pool = _pools[poolKey];
                _pools.Remove(poolKey, out _);
                _poolOptions.Remove(poolKey, out _);

                foreach (var wrapper in pool)
                {
                    try
                    {
                        wrapper.DbConnection.Dispose();
                    }
                    catch (Exception ex)
                    {
                        ReleaseConnectionError?.Invoke(ex, wrapper.DbConnection);
                    }
                    _connInfos.Remove((wrapper.DbConnection as SqlConnection).ClientConnectionId, out _);
                }

                pool.Clear();
            }

            return Task.CompletedTask;
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
                var pool = _pools[poolKey];
                var poolOption = _poolOptions[poolKey];

                if (HasFullOfConnections(pool, poolOption)) return false;

                try
                {
                    var dbConn = new SqlConnection(poolOption.ConnectionString);
                    var wrapper = new PooledDbConnectionWrapper(dbConn, poolKey);

                    dbConn.StateChange += async (sender, e) => await DbConn_StateChangeAsync(sender, e, wrapper);

                    dbConn.Open();

                    AddNewConnection(pool, wrapper);
                }
                catch (Exception ex)
                {
                    NewConnectionError?.Invoke(ex, poolKey);
                    throw ex;
                }
            }

            return true;
        }

        private async Task DbConn_StateChangeAsync(object sender, StateChangeEventArgs e,
            PooledDbConnectionWrapper wrapper)
        {
            if (disposedValue || wrapper.DbConnection.IsOpening()
                || HasExceededLifetime(
                    _connInfos[(wrapper.DbConnection as SqlConnection).ClientConnectionId],
                    _poolOptions[wrapper.PoolKey])) return;

            await TryReturnToPoolAsync(wrapper);
        }

        private void Watch()
        {
            void Try(Action action, PooledDbConnectionWrapper wrapper)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    WatcherThreadError?.Invoke(ex, wrapper.DbConnection);
                }
            }

            while (!disposedValue)
            {
                Thread.Sleep(TimeSpan.FromMinutes(_options.WatchIntervalInMinutes));

                foreach (var poolKey in _poolOptions.Keys)
                {
                    var pool = _pools[poolKey];
                    var poolOption = _poolOptions[poolKey];

                    PooledDbConnectionWrapper currentConn;
                    ConnectionInfo currentConnInfo;

                    var disposes = new List<DbConnection>();
                    var opens = new List<PooledDbConnectionWrapper>();
                    var validConns = new List<PooledDbConnectionWrapper>();

                    lock (_poolsLock)
                    {
                        while (TryGetConnection(pool, out currentConn, out currentConnInfo))
                        {
                            if (!currentConn.DbConnection.IsOpening())
                            {
                                if (HasRedundantConnections(pool, poolOption))
                                    disposes.Add(currentConn.DbConnection);
                                else
                                    opens.Add(currentConn);
                            }
                            else if (!HasExceededLifetime(currentConnInfo, poolOption)
                                || IsLackOfConnections(pool, poolOption))
                                validConns.Add(currentConn);
                        }

                        foreach (var conn in validConns)
                            AddNewConnection(pool, conn);
                    }

                    foreach (var needDisposeConn in disposes)
                    {
                        Try(() => needDisposeConn.Dispose(), currentConn);
                    }

                    foreach (var needOpenConn in opens)
                    {
                        Try(() => RetryAction((retryCount) =>
                        {
                            try
                            {
                                lock (_poolsLock)
                                {
                                    if (IsLackOfConnections(pool, poolOption))
                                    {
                                        needOpenConn.DbConnection.Open();
                                        AddNewConnection(pool, needOpenConn);
                                    }
                                }
                                return true;
                            }
                            catch (Exception ex)
                            {
                                WatcherThreadError?.Invoke(ex, currentConn.DbConnection);
                                return false;
                            }
                        }, poolOption), currentConn);
                    }

                    var addNewSuccess = true;

                    while (IsLackOfConnections(pool, poolOption) && addNewSuccess)
                    {
                        Try(() => RetryAction((retryCount) =>
                        {
                            try
                            {
                                addNewSuccess = SetupNewConnection(poolKey);
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }, poolOption), currentConn);
                    }
                }
            }
        }

        public Task<bool> TryReturnToPoolAsync(DbConnection connection)
        {
            if (disposedValue || !connection.CanOpen()) return Task.FromResult(false);

            if (connection is SqlConnection == false) throw new InvalidOperationException("Unsupported connection");

            var sqlConn = connection as SqlConnection;

            var poolKey = GetPoolKeyFromConnStr(sqlConn.ConnectionString);

            if (!_poolOptions.ContainsKey(poolKey)) return Task.FromResult(false);

            var wrapper = new PooledDbConnectionWrapper(connection, poolKey);

            return TryReturnToPoolAsync(wrapper);
        }

        private Task<bool> TryReturnToPoolAsync(PooledDbConnectionWrapper wrapper)
        {
            if (disposedValue || !wrapper.DbConnection.CanOpen()) return Task.FromResult(false);

            return Task.Run(() =>
            {
                if (disposedValue || !wrapper.DbConnection.CanOpen())
                    return false;

                var poolKey = wrapper.PoolKey;
                var poolOption = _poolOptions[poolKey];
                var pool = _pools[poolKey];
                var inPool = false;

                RetryAction((retryCount) =>
                {
                    try
                    {
                        lock (_poolsLock)
                        {
                            if (!HasFullOfConnections(pool, poolOption))
                            {
                                if (wrapper.IsInPool)
                                {
                                    if (HasExceededLifetime(
                                        _connInfos[(wrapper.DbConnection as SqlConnection).ClientConnectionId],
                                        poolOption)) return true;

                                    if (!wrapper.DbConnection.IsOpening() && wrapper.DbConnection.CanOpen())
                                        wrapper.DbConnection.Open();
                                }
                                else
                                {
                                    if (!wrapper.DbConnection.IsOpening() && wrapper.DbConnection.CanOpen())
                                        wrapper.DbConnection.Open();

                                    AddNewConnection(pool, wrapper);
                                }

                                inPool = true;
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

                return inPool;
            });
        }

        private string GetPoolKeyFromConnStr(string connStr)
        {
            var parts = connStr.Split(';').Where(part =>
                !part.Trim().StartsWith(SqlConnectionConsts.Options.Password)).ToArray();

            return string.Join(';', parts);
        }

        private bool HasExceededLifetime(ConnectionInfo info, ConnectionPoolOptions poolOption)
        {
            return (DateTimeOffset.UtcNow - info.CreatedTime).TotalMinutes >= poolOption.LifetimeInMinutes;
        }

        private bool HasFullOfConnections(ConcurrentQueue<PooledDbConnectionWrapper> pool,
            ConnectionPoolOptions poolOption)
        {
            return (pool.Count >= poolOption.MaxPoolSize);
        }

        private bool IsLackOfConnections(ConcurrentQueue<PooledDbConnectionWrapper> pool,
            ConnectionPoolOptions poolOption)
        {
            return (pool.Count < poolOption.MinPoolSize);
        }

        private bool HasRedundantConnections(ConcurrentQueue<PooledDbConnectionWrapper> pool,
            ConnectionPoolOptions poolOption)
        {
            return (pool.Count > poolOption.MinPoolSize);
        }

        private bool RetryAction(Func<int, bool> action, ConnectionPoolOptions options)
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

        private bool TryGetConnection(ConcurrentQueue<PooledDbConnectionWrapper> pool,
            out PooledDbConnectionWrapper wrapper,
            out ConnectionInfo connectionInfo)
        {
            connectionInfo = default;
            var hasConn = pool.TryDequeue(out wrapper);

            if (hasConn)
            {
                _connInfos.Remove((wrapper.DbConnection as SqlConnection).ClientConnectionId, out connectionInfo);
                wrapper.IsInPool = false;
            }

            return hasConn;
        }

        private void AddNewConnection(ConcurrentQueue<PooledDbConnectionWrapper> pool, PooledDbConnectionWrapper wrapper)
        {
            pool.Enqueue(wrapper);
            _connInfos[(wrapper.DbConnection as SqlConnection).ClientConnectionId] = new ConnectionInfo()
            {
                CreatedTime = DateTimeOffset.UtcNow,
                Wrapper = wrapper
            };
            wrapper.IsInPool = true;
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
