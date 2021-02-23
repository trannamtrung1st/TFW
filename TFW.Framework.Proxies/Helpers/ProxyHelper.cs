using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Proxies.Helpers
{
    public static class ProxyHelper
    {
        public static object CreateSimpleInterfaceProxy(this object target, Type interfaceType, params IInterceptor[] interceptors)
        {
            var proxy = new ProxyGenerator()
                .CreateInterfaceProxyWithTarget(interfaceType, target, interceptors);

            return proxy;
        }

        public static T CreateSimpleInterfaceProxy<T>(this T target, params IInterceptor[] interceptors) where T : class
        {
            var proxy = new ProxyGenerator()
                .CreateInterfaceProxyWithTarget<T>(target, interceptors);

            return proxy;
        }
    }
}
