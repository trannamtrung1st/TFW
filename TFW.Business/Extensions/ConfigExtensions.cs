using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TFW.Business.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ConfigureBusiness(this IServiceCollection services)
        {
            // configure Business 
            return services;
        }
    }
}
