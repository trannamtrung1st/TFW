using System;
using System.Collections.Generic;

namespace TFW.Framework.DI
{
    internal class KeyedService : IDisposable
    {
        private object _service;

        public void Dispose()
        {
            (_service as IDisposable)?.Dispose();
        }

        public object GetService(KeyedServiceInfo info, IServiceProvider provider, object args, bool required)
        {
            var result = ResolveService(info, provider, args, required);

            if (result.Service == null) throw new Exception("Can not resolve service");

            if (result.Save == true) _service = result.Service;

            return result.Service;
        }

        private (object Service, bool? Save) ResolveService(KeyedServiceInfo info, IServiceProvider provider, object args, bool required)
        {
            if (_service != null) return (_service, false);

            (Type KeyedType, Type Type, Func<IServiceProvider, object> Factory) typeInfo;

            if (!info.Types.TryGetValue(args, out typeInfo))
                if (!required) return default;
                else throw new KeyNotFoundException();

            if (typeInfo.Factory != null)
                return (typeInfo.Factory(provider), true);

            return (info.CachedObjectFactory[typeInfo.Type](provider, null), true);
        }
    }

    internal class KeyedService<T> : KeyedService
    {
    }
}
