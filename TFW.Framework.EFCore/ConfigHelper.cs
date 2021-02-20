using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.EFCore.Migration;

namespace TFW.Framework.EFCore
{
    public static class ConfigHelper
    {
        public static IServiceCollection AddDefaultDbMigrator(this IServiceCollection services)
        {
            return services.AddSingleton<IDbMigrator>(new DefaultDbMigrator());
        }
    }
}
