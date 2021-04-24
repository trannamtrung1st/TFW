using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using TFW.Docs.Cross.Entities;
using TFW.Framework.Common.Helpers;
using TFW.Framework.DI.Attributes;

namespace TFW.Docs.Cross.Providers
{
    [SingletonService(ServiceType = typeof(IDynamicLinkCustomTypeProvider))]
    public class DynamicLinqEntityTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public override HashSet<Type> GetCustomTypes() => _entityTypes;

        private static readonly HashSet<Type> _entityTypes;

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
