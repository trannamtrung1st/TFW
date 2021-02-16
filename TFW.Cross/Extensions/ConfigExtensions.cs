using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ConfigureCross(this IServiceCollection services)
        {
            // configure Cross
            return services;
        }
    }
}
