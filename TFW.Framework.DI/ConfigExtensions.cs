using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TFW.Framework.DI
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ScanServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
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
                    var serviceDescriptor = attr.BuildServiceDescriptor(typeObj.Type);

                    // Check is service already register from difference implementation => throw exception
                    var isAlreadyDifferenceImplementation = services.Any(
                        x =>
                            x.ServiceType.FullName == serviceDescriptor.ServiceType.FullName &&
                            x.ImplementationType != serviceDescriptor.ImplementationType);

                    if (isAlreadyDifferenceImplementation)
                    {
                        var implementationRegister =
                            services.Single(x => x.ServiceType.FullName == serviceDescriptor.ServiceType.FullName)
                                .ImplementationType;

                        throw new ConflictServiceRegistrationException(
                            $"Conflict register, ${serviceDescriptor.ImplementationType} try to register for {serviceDescriptor.ServiceType.FullName}. It already register by {implementationRegister.FullName} before.");
                    }

                    // Check is service already register from same implementation => remove existing,
                    // replace by new one life time cycle
                    var isAlreadySameImplementation = services.Any(
                        x =>
                            x.ServiceType.FullName == serviceDescriptor.ServiceType.FullName &&
                            x.ImplementationType == serviceDescriptor.ImplementationType);

                    if (isAlreadySameImplementation)
                        services = services.Replace(serviceDescriptor);
                    else
                        services.Add(serviceDescriptor);
                }
            }
            return services;
        }
    }
}