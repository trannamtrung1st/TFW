using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Data;
using TFW.Framework.Configuration;
using TFW.Framework.Data;

namespace TFW.Docs.WebApi
{
    internal static class StartupConfig
    {

        public static IServiceCollection AddAppDbContext(this IServiceCollection services, ISecretsManager secretsManager)
        {
            string connStr = secretsManager.Get(DataConsts.ConnStrKey);

            if (connStr is null) throw new ArgumentNullException(nameof(connStr));

            services.AddNullConnectionPoolManager()
                .AddDbContext<DataContext>(options => options
                    .UseSqlServer(connStr)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            return services;
        }

    }
}
