using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TFW.Framework.DI.Attributes;
using TFW.Framework.DI.Exceptions;

namespace TFW.Framework.DI
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddKeyedServiceManager(this IServiceCollection services,
            out IKeyedServiceManager manager)
        {
            manager = new KeyedServiceManager();

            return services.AddSingleton(manager);
        }

        public static IServiceCollection SetKeyed<ServiceType, ImplType>(this IServiceCollection services,
            IKeyedServiceManager manager, object key, Func<IServiceProvider, ImplType> factory = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped) where ImplType : ServiceType
        {
            Func<IServiceProvider, object> finalFactory = null;

            if (factory != null) finalFactory = (provider) => factory(provider);

            return services.SetKeyed(manager, typeof(ServiceType), typeof(ImplType), key, finalFactory, lifetime);
        }

        public static IServiceCollection SetKeyed(this IServiceCollection services,
            IKeyedServiceManager manager,
            Type serviceType, Type implType, object key, Func<IServiceProvider, object> factory = null,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (!serviceType.IsAssignableFrom(implType))
                throw new InvalidOperationException("ServiceType is not assignable from ImplType");

            var keyedType = typeof(KeyedService<>).MakeGenericType(implType);

            if (!manager.ServiceTypes.ContainsKey(serviceType))
                manager.ServiceTypes[serviceType] = new KeyedServiceInfo
                {
                    CachedObjectFactory = new Dictionary<Type, ObjectFactory>(),
                    ServiceLifetime = lifetime,
                    Types = new Dictionary<object, (Type, Type, Func<IServiceProvider, object>)>()
                };

            var info = manager.ServiceTypes[serviceType];
            info.Types[key] = (keyedType, implType, factory);
            info.CachedObjectFactory[implType] = ActivatorUtilities.CreateFactory(implType, new Type[] { });

            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(keyedType);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(keyedType);
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton(keyedType);
                    break;
            }

            return services;
        }

        public static IServiceCollection AddServiceInjector(this IServiceCollection services,
            IEnumerable<Assembly> assemblies, out IServiceInjector injector)
        {
            var concreteInjector = new ServiceInjector();
            injector = concreteInjector;

            concreteInjector.Register(assemblies);

            return services.AddSingleton(injector);
        }

        public static IServiceCollection AddServiceInjector(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            return services.AddServiceInjector(assemblies, out _);
        }

        public static IServiceCollection ScanServices(this IServiceCollection services, IEnumerable<Assembly> assemblies,
            IServiceInjector serviceInjector = null)
        {
            var serviceTypes = assemblies.SelectMany(o => o.GetTypes()).Select(o => new
            {
                Type = o,
                Attributes = o.GetCustomAttributes<ServiceAttribute>().ToArray()
            }).Where(o => o.Attributes.Any()).ToArray();

            foreach (var typeObj in serviceTypes)
            {
                foreach (var attr in typeObj.Attributes)
                {
                    var useServiceInjector = serviceInjector?.RegisteredTypes.Contains(typeObj.Type) == true;
                    var serviceDescriptor = attr.BuildServiceDescriptor(typeObj.Type, useServiceInjector);

                    var isAlreadyRegistered = services.Any(
                        x => x.ServiceType.FullName == serviceDescriptor.ServiceType.FullName);

                    if (isAlreadyRegistered)
                    {
                        if (attr.ThrowIfExists)
                            throw new ServiceRegistrationException(
                                $"This descriptor {serviceDescriptor.ImplementationType} must be the only " +
                                $"registration for {serviceDescriptor.ServiceType.FullName}");

                        if (attr.Replace)
                            services = services.Replace(serviceDescriptor);
                        else services.Add(serviceDescriptor);
                    }
                    else
                    {
                        services.Add(serviceDescriptor);
                    }
                }
            }

            return services;
        }
    }
}