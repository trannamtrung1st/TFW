using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TFW.Framework.Common.Helpers;
using TFW.Framework.Web.Attributes;

namespace TFW.Framework.Web.Options
{
    public class FrameworkOptions
    {
        public IReadOnlyDictionary<object, Type[]> ShouldSkipFilterTypesMap { get; internal set; }

        internal void CopyFrom(FrameworkOptions options)
        {
            ShouldSkipFilterTypesMap = options.ShouldSkipFilterTypesMap;
        }
    }

    public class FrameworkOptionsBuilder
    {
        private Dictionary<object, Type[]> _shouldSkipFilterTypesMap;
        public IDictionary<object, Type[]> ShouldSkipFilterTypesMap => _shouldSkipFilterTypesMap;

        public FrameworkOptionsBuilder()
        {
            _shouldSkipFilterTypesMap = new Dictionary<object, Type[]>();
        }

        public FrameworkOptions Build()
        {
            return new FrameworkOptions
            {
                ShouldSkipFilterTypesMap = _shouldSkipFilterTypesMap
            };
        }

        public void ScanShouldSkipFilterTypes(Assembly controllerAssembly, IEnumerable<string> controllerNs, bool includeSubns = true)
        {
            var types = controllerNs.SelectMany(ns => ReflectionHelper.GetTypesOfNamespace(
                    ns, controllerAssembly, includeSubns));

            var skipAttrType = typeof(ShouldSkipFilterAttribute);

            foreach (var type in types)
            {
                var skipAttr = type.GetCustomAttribute<ShouldSkipFilterAttribute>();

                if (skipAttr != null)
                    _shouldSkipFilterTypesMap[type] = skipAttr.SkippedFilterTypes;

                var typeMethods = type.GetMethods().Select(o => new
                {
                    Method = o,
                    SkipAttr = o.GetCustomAttribute<ShouldSkipFilterAttribute>()
                }).Where(o => o.SkipAttr != null);

                foreach (var methodObj in typeMethods)
                    _shouldSkipFilterTypesMap[methodObj.Method] = methodObj.SkipAttr.SkippedFilterTypes;
            }
        }
    }
}
