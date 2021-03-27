using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TFW.Framework.DI.Attributes;
using TFW.Framework.DI.Exceptions;

namespace TFW.Framework.DI
{

    public static class ConfigHelper
    {
        public static IServiceCollection AddServiceInjector(this IServiceCollection services,
            IEnumerable<Assembly> assemblies, out IServiceInjector injector)
        {
            var concreteInjector = new ServiceInjector();
            injector = concreteInjector;

            concreteInjector.Register(assemblies);

            return services.AddSingleton(injector);
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