using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.WebAPI.Bindings;

namespace TFW.Framework.WebAPI.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection AddDefaultDateTimeModelBinder(this IServiceCollection services)
        {
            return services.AddSingleton(new DefaultDateTimeModelBinder());
        }
    }
}
