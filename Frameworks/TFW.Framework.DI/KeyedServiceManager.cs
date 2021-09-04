using System;
using System.Collections.Generic;

namespace TFW.Framework.DI
{
    public interface IKeyedServiceManager
    {
        IDictionary<Type, KeyedServiceInfo> ServiceTypes { get; }
    }

    internal class KeyedServiceManager : IKeyedServiceManager
    {
        public IDictionary<Type, KeyedServiceInfo> ServiceTypes { get; }

        public KeyedServiceManager()
        {
            ServiceTypes = new Dictionary<Type, KeyedServiceInfo>();
        }
    }
}
