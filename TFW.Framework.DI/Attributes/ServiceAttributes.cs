using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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

        public virtual ServiceDescriptor BuildServiceDescriptor(Type type)
        {
            var serviceType = ServiceType ?? type;

            return new ServiceDescriptor(serviceType, type, Lifetime);
        }
    }
}
