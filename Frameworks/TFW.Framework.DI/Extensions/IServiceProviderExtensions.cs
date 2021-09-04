using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TFW.Framework.DI.Extensions
{
    public static class IServiceProviderExtensions
    {
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
            var manager = provider.GetService<IKeyedServiceManager>();

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
