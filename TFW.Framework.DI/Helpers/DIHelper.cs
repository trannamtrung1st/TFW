using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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

        public static T GetService<T>(this IServiceProvider provider, object args)
        {
            return (T)provider.GetService(typeof(T), args);
        }

        public static T GetRequiredService<T>(this IServiceProvider provider, object args)
        {
            return (T)provider.GetRequiredService(typeof(T), args);
        }

        public static object GetService(this IServiceProvider provider, Type type, object args)
        {
            var manager = provider.GetService<KeyedServiceManager>();

            if (manager == null) return null;

            KeyedServiceInfo info;
            if (!manager.ServiceTypes.TryGetValue(type, out info)) return null;

            if (!info.Types.ContainsKey(args)) return null;

            var condition = provider.GetService(info.Types[args].KeyedType) as KeyedService;

            return condition.GetService(info, provider, args, false);
        }

        public static object GetRequiredService(this IServiceProvider provider, Type type, object args)
        {
            var manager = provider.GetRequiredService<IKeyedServiceManager>();

            KeyedServiceInfo info;
            if (!manager.ServiceTypes.TryGetValue(type, out info))
                throw new KeyNotFoundException(type.ToString());

            if (!info.Types.ContainsKey(args))
                throw new KeyNotFoundException(args?.ToString());

            var condition = provider.GetRequiredService(info.Types[args].KeyedType) as KeyedService;

            return condition.GetService(info, provider, args, true);
        }
    }
}