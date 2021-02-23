using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Cross.Providers;
using TFW.Data.Providers;
using TFW.WebAPI.Providers;

namespace TFW.WebAPI.Helpers
{
    public static class ConfigHelper
    {
        public static IServiceCollection AddHttpUnitOfWorkProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IUnitOfWorkProvider, HttpUnitOfWorkProvider>();
        }

        public static IServiceCollection AddHttpBusinessContextProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IBusinessContextProvider, HttpBusinessContextProvider>();
        }

    }
}
