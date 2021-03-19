using System;
using System.Data.Common;
using System.Threading.Tasks;
using TFW.Framework.Data.Options;
using TFW.Framework.Data.Wrappers;

namespace TFW.Framework.Data
{
    public delegate void TryReturnToPoolErrorEventHandler(Exception ex, int tryCount);
    public delegate void WatcherThreadErrorEventHandler(Exception ex, DbConnection dbConnection);
    public delegate void NewConnectionErrorEventHandler(Exception ex, string poolKey);

    public interface IDbConnectionPoolManager : IDisposable, IAsyncDisposable
    {
        bool IsNullObject { get; }

        Task<DbConnection> GetDbConnectionAsync(string connStrKey);
        Task InitDbConnectionAsync(ConnectionPoolOptions options, string poolKey = null);
        Task ReleasePoolAsync(string poolKey);
        Task ReleaseAllPoolsAsync();
        Task TryReturnToPoolAsync(DbConnection connection);

        event TryReturnToPoolErrorEventHandler TryReturnToPoolError;
        event WatcherThreadErrorEventHandler WatcherThreadError;
        event NewConnectionErrorEventHandler NewConnectionError;
    }
}
