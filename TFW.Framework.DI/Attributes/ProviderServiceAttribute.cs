using Microsoft.Extensions.DependencyInjection;
using System;

namespace TFW.Framework.DI.Attributes
{
    public class ProviderServiceAttribute : ServiceAttribute
    {
        public ProviderServiceAttribute(ServiceLifetime lifetime) : base(lifetime)
        {
        }

        public override ServiceDescriptor BuildServiceDescriptor(Type type)
        {
            var serviceType = ServiceType ?? type;

            return new ServiceDescriptor(serviceType, o => o.GetRequiredService(type), Lifetime);
        }
    }
}
