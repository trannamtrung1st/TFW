using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TFW.Business.Helpers
{
    public static class ConfigHelper
    {
        public static IServiceCollection ConfigureBusiness(this IServiceCollection services)
        {
            // configure Business 
            return services;
        }
    }
}
