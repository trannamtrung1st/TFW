using Microsoft.Extensions.DependencyInjection;
using System;

namespace TFW.Framework.DI.Attributes
{
    public class ProviderServiceAttribute : ServiceAttribute
    {
        public ProviderServiceAttribute(ServiceLifetime lifetime) : base(lifetime)
        {
        }

        internal override ServiceDescriptor BuildServiceDescriptor(Type type, bool useServiceInjector)
        {
            var serviceType = ServiceType ?? type;

            if (!useServiceInjector)
                return new ServiceDescriptor(serviceType, provider => provider.GetRequiredService(type), Lifetime);

            if (serviceType == type) throw new InvalidOperationException("Loop detected");

            return new ServiceDescriptor(serviceType, DIHelper.BuildInjectedFactory(type, true), Lifetime);
        }
    }
}
