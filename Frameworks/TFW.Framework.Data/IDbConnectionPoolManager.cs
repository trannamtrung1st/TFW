using System;
using System.Data.Common;
using System.Threading.Tasks;
using TFW.Framework.Data.Options;

namespace TFW.Framework.Data
{
    public delegate void TryReturnToPoolErrorEventHandler(Exception ex, int tryCount);
    public delegate void WatcherThreadErrorEventHandler(Exception ex, DbConnection dbConnection);
    public delegate void NewConnectionErrorEventHandler(Exception ex, string poolKey);
    public delegate void ReleaseConnectionErrorEventHandler(Exception ex, DbConnection dbConnection);

    public interface IDbConnectionPoolManager : IDisposable, IAsyncDisposable
    {
        bool IsNullObject { get; }

        Task<DbConnection> GetDbConnectionAsync(string poolKey);
        Task<string> InitDbConnectionAsync(ConnectionPoolOptions options);
        Task ReleasePoolAsync(string poolKey);
        Task ReleaseAllPoolsAsync();
        Task<bool> TryReturnToPoolAsync(DbConnection connection);

        event TryReturnToPoolErrorEventHandler TryReturnToPoolError;
        event WatcherThreadErrorEventHandler WatcherThreadError;
        event NewConnectionErrorEventHandler NewConnectionError;
        event ReleaseConnectionErrorEventHandler ReleaseConnectionError;
    }
}
