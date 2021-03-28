using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace TFW.Framework.DI
{
    public struct KeyedServiceInfo
    {
        public ServiceLifetime ServiceLifetime { get; set; }
        public IDictionary<object, (Type KeyedType, Type Type, Func<IServiceProvider, object> Factory)> Types { get; set; }
        public IDictionary<Type, ObjectFactory> CachedObjectFactory { get; set; }
    }
}
