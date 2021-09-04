using System.Data.Common;
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
        public event ReleaseConnectionErrorEventHandler ReleaseConnectionError;

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }

        public Task<DbConnection> GetDbConnectionAsync(string poolKey)
        {
            return default;
        }

        public Task<string> InitDbConnectionAsync(ConnectionPoolOptions options)
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

        public Task<bool> TryReturnToPoolAsync(DbConnection connection)
        {
            return default;
        }
    }
}
