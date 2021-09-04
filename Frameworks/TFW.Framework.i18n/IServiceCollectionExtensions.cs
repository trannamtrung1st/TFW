using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TFW.Framework.Common.Helpers;
using TFW.Framework.i18n.Localization;
using TFW.Framework.i18n.Localization.Factory;
using TFW.Framework.i18n.Options;

namespace TFW.Framework.i18n
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ScanInMemoryResources(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var resObjects = ReflectionHelper.GetAllTypesAssignableTo(typeof(IInMemoryResources), assemblies)
                .Select(type => Activator.CreateInstance(type) as IInMemoryResources).ToArray();

            return services.Configure<InMemoryLocalizerOptions>(options =>
            {
                foreach (var resObj in resObjects)
                {
                    options.Resources[resObj.SourceType] = resObj.Resources;
                }
            });
        }

        public static IServiceCollection AddInMemoryLocalizer(this IServiceCollection services)
        {
            services.TryAdd(new ServiceDescriptor(
                typeof(IStringLocalizerFactory),
                typeof(InMemoryStringLocalizerFactory),
                ServiceLifetime.Singleton));

            return services;
        }
    }
}
