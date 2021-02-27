using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using TFW.Cross.Entities;
using TFW.Framework.Common.Helpers;
using TFW.Framework.DI.Attributes;

namespace TFW.Cross.Providers
{
    [SingletonService(ServiceType = typeof(IDynamicLinkCustomTypeProvider))]
    public class DynamicLinqEntityTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public override HashSet<Type> GetCustomTypes() => _entityTypes;

        private static HashSet<Type> _entityTypes;

        static DynamicLinqEntityTypeProvider()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var entityTypes = ReflectionHelper.GetTypesOfNamespace(
                typeof(AppUser).Namespace, assembly, includeSubns: true);
            var modelTypes = ReflectionHelper.GetTypesOfNamespace(
                typeof(NamespaceModel).Namespace, assembly, includeSubns: true);

            _entityTypes = entityTypes.Concat(modelTypes).ToHashSet();
        }
    }
}
