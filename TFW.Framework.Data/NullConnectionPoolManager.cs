using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.Data.Options;

namespace TFW.Framework.Data
{
    public class NullConnectionPoolManager : IDbConnectionPoolManager
    {
        public bool IsNullObject => true;

        public event TryReturnToPoolErrorEventHandler TryReturnToPoolError;
        public event WatcherThreadErrorEventHandler WatcherThreadError;
        public event NewConnectionErrorEventHandler NewConnectionError;

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }

        public Task<DbConnection> GetDbConnectionAsync(string connStrKey)
        {
            return default;
        }

        public Task InitDbConnectionAsync(ConnectionPoolOptions options, string poolKey = null)
        {
            return default;
        }

        public Task ReleaseAllPoolsAsync()
        {
            return default;
        }

        public Task ReleasePoolAsync(string poolKey)
        {
            return default;
        }

        public Task TryReturnToPoolAsync(DbConnection connection)
        {
            return default;
        }
    }
}
