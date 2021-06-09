using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TFW.Framework.DI
{
    public static class DIHelper
    {
        //[ActivatorUtilitiesConstructor]: use this to indicate the constructor if there are multiple
        public static Func<IServiceProvider, object> BuildInjectedFactory(Type type)
        {
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
        public static Func<IServiceProvider, T> BuildInjectedFactory<T>()
        {
            var objectFactory = ActivatorUtilities.CreateFactory(typeof(T), new Type[] { });

            return (provider) =>
            {
                var service = objectFactory(provider, null);
                var injector = provider.GetRequiredService<IServiceInjector>();

                injector.Inject(service, provider);

                return (T)service;
            };
        }

        public static Func<IServiceProvider, object> BuildInjectedFactory(Func<IServiceProvider, object> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            return (provider) =>
            {
                var service = factory(provider);
                var injector = provider.GetRequiredService<IServiceInjector>();

                injector.Inject(service, provider);

                return service;
            };
        }

        public static Func<IServiceProvider, T> BuildInjectedFactory<T>(Func<IServiceProvider, T> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            return (provider) =>
            {
                var service = factory(provider);
                var injector = provider.GetRequiredService<IServiceInjector>();

                injector.Inject(service, provider);

                return service;
            };
        }
    }
}