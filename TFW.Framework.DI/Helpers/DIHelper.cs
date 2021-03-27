using Microsoft.Extensions.DependencyInjection;
using System;

namespace TFW.Framework.DI
{
    public static class DIHelper
    {
        //[ActivatorUtilitiesConstructor]: use this to indicate the constructor if there are multiple
        public static Func<IServiceProvider, object> BuildInjectedFactory(Type type, bool useServiceProvider = false)
        {
            if (useServiceProvider)
            {
                return (provider) =>
                {
                    var service = provider.GetRequiredService(type);
                    var injector = provider.GetRequiredService<IServiceInjector>();

                    injector.Inject(service, provider);

                    return service;
                };
            }

            var objectFactory = ActivatorUtilities.CreateFactory(type, new Type[] { });

            return (provider) =>
            {
                var service = objectFactory(provider, null);
                var injector = provider.GetRequiredService<IServiceInjector>();

                injector.Inject(service, provider);

                return service;
            };
        }

        //[ActivatorUtilitiesConstructor]: use this to indicate the constructor if there are multiple
        public static Func<IServiceProvider, T> BuildInjectedFactory<T>(bool useServiceProvider = false)
        {
            if (useServiceProvider)
            {
                return (provider) =>
                {
                    var service = provider.GetRequiredService<T>();
                    var injector = provider.GetRequiredService<IServiceInjector>();

                    injector.Inject(service, provider);

                    return service;
                };
            }

            var objectFactory = ActivatorUtilities.CreateFactory(typeof(T), new Type[] { });

            return (provider) =>
            {
                var service = objectFactory(provider, null);
                var injector = provider.GetRequiredService<IServiceInjector>();

                injector.Inject(service, provider);

                return (T)service;
            };
        }
    }
}