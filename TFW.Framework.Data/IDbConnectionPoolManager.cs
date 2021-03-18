using System;
using System.Data.Common;
using System.Threading.Tasks;
using TFW.Framework.Data.Options;

namespace TFW.Framework.Data
{
    public delegate void RetryAddToPoolErrorEventHandler(Exception ex, int tryCount);

    public interface IDbConnectionPoolManager : IDisposable, IAsyncDisposable
    {
        Task<DbConnection> GetDbConnectionAsync(string connStrKey, bool createNonPooledConnIfExceedLimit = false);
        Task InitDbConnectionAsync(SqlConnectionPoolOptions options, string poolKey = null);
        Task ReleasePoolAsync(string poolKey);
        Task ReleaseAllPoolsAsync();
        Task RetryAddToPoolAsync(DbConnection connection);

        event RetryAddToPoolErrorEventHandler RetryAddToPoolError;
    }
}
