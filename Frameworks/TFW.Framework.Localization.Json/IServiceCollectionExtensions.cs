﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace TFW.Framework.Localization.Json
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonLocalizer(this IServiceCollection services)
        {
            services.TryAdd(new ServiceDescriptor(
                typeof(IStringLocalizerFactory),
                typeof(JsonStringLocalizerFactory),
                ServiceLifetime.Singleton));

            return services;
        }
    }
}
