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
            Func<IServiceProvider, object> factory = provider => provider.GetRequiredService(type);

            if (!useServiceInjector)
                return new ServiceDescriptor(serviceType, factory, Lifetime);

            if (serviceType == type) throw new InvalidOperationException("Loop detected");

            return new ServiceDescriptor(serviceType, DIHelper.BuildInjectedFactory(factory), Lifetime);
        }
    }
}
