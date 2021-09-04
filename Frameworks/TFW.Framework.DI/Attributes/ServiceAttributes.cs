using Microsoft.Extensions.DependencyInjection;
using System;

namespace TFW.Framework.DI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class ServiceAttribute : Attribute
    {
        protected ServiceAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }

        public ServiceLifetime Lifetime { get; }

        public Type ServiceType { get; set; }

        // true: replace/ false: add new
        public bool Replace { get; set; }

        public bool ThrowIfExists { get; set; }

        internal virtual ServiceDescriptor BuildServiceDescriptor(Type type, bool useServiceInjector)
        {
            var serviceType = ServiceType ?? type;

            if (!useServiceInjector)
                return new ServiceDescriptor(serviceType, type, Lifetime);

            return new ServiceDescriptor(serviceType, DIHelper.BuildInjectedFactory(type), Lifetime);
        }
    }
}
