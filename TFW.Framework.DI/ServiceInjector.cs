using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TFW.Framework.DI.Attributes;

namespace TFW.Framework.DI
{
    public interface IServiceInjector
    {
        void Inject(object obj, IServiceProvider serviceProvider);
        IEnumerable<Type> RegisteredTypes { get; }
    }

    internal struct InjectedTypeInfo
    {
        public (MethodInfo Setter, Type Type, bool Required)[] PropertySetters { get; set; }
    }

    internal class ServiceInjector : IServiceInjector
    {
        private readonly IDictionary<Type, InjectedTypeInfo> _mappings;

        public IEnumerable<Type> RegisteredTypes => _mappings.Keys;

        public ServiceInjector()
        {
            _mappings = new Dictionary<Type, InjectedTypeInfo>();
        }

        public void Register(IEnumerable<Assembly> assemblies)
        {
            var serviceTypes = assemblies.SelectMany(o => o.GetTypes()).ToArray();

            foreach (var type in serviceTypes)
            {
                var allProps = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var injectableProps = allProps.Where(prop =>
                        prop.IsDefined(typeof(InjectAttribute), false) && prop.SetMethod != null)
                            .Select(prop =>
                            (
                                prop.SetMethod,
                                prop.PropertyType,
                                prop.GetCustomAttribute<InjectAttribute>(false).Required
                            )).ToArray();

                if (injectableProps.Length > 0)
                {
                    var info = new InjectedTypeInfo()
                    {
                        PropertySetters = injectableProps
                    };

                    _mappings[type] = info;
                }
            }
        }

        public void Inject(object obj, IServiceProvider serviceProvider)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var type = obj.GetType();

            if (_mappings.ContainsKey(type))
            {
                var info = _mappings[type];

                foreach (var p in info.PropertySetters)
                {
                    p.Setter.Invoke(obj, new[]
                    {
                        p.Required? serviceProvider.GetRequiredService(p.Type) : serviceProvider.GetService(p.Type)
                    });
                }
            }
        }
    }
}
