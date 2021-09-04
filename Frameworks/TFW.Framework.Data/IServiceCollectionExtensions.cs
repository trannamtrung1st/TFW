using Microsoft.Extensions.DependencyInjection;
using System;
using TFW.Framework.Data.Options;

namespace TFW.Framework.Data
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNullConnectionPoolManager(this IServiceCollection services)
        {
            return services.AddSingleton<IDbConnectionPoolManager>(new NullConnectionPoolManager());
        }

        public static IServiceCollection AddSqlConnectionPoolManager(this IServiceCollection services,
            out IDbConnectionPoolManager connPoolManager,
            Action<SqlConnectionPoolManagerOptions> configAction = null,
            Action<IDbConnectionPoolManager> initAction = null)
        {
            var options = new SqlConnectionPoolManagerOptions();
            configAction?.Invoke(options);

            connPoolManager = new SqlConnectionPoolManager(options);
            initAction?.Invoke(connPoolManager);

            return services.AddSingleton(connPoolManager);
        }
    }
}
