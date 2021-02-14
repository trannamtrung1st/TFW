using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TFW.Framework.DI
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

        public ServiceDescriptor BuildServiceDescriptor(Type type)
        {
            var serviceType = ServiceType ?? type;
            return new ServiceDescriptor(serviceType, type, Lifetime);
        }
    }

    public class ScopedServiceAttribute : ServiceAttribute
    {
        public ScopedServiceAttribute() : base(ServiceLifetime.Scoped)
        {
        }
    }

    public class TransientServiceAttribute : ServiceAttribute
    {
        public TransientServiceAttribute() : base(ServiceLifetime.Transient)
        {
        }
    }

    public class SingletonServiceAttribute : ServiceAttribute
    {
        public SingletonServiceAttribute() : base(ServiceLifetime.Singleton)
        {
        }
    }
}
