using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.EFCore
{
    public static class ConfigExtensions
    {
        public static IServiceCollection AddDefaultDbMigrator(this IServiceCollection services)
        {
            return services.AddSingleton<IDbMigrator>(new DefaultDbMigrator());
        }
    }
}
