using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Helpers
{
    public static class ConfigHelper
    {
        public static IServiceCollection ConfigureCross(this IServiceCollection services)
        {
            // configure Cross
            return services;
        }
    }
}
