using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using TFW.Cross.Entities;
using TFW.Cross.Models;
using TFW.Framework.Common.Helpers;
using TFW.Framework.DI;

namespace TFW.Cross.Commons
{
    [SingletonService(ServiceType = typeof(IDynamicLinkCustomTypeProvider))]
    public class DynamicLinqEntityTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public override HashSet<Type> GetCustomTypes() => _entityTypes;

        private static HashSet<Type> _entityTypes;

        static DynamicLinqEntityTypeProvider()
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            var entityTypes = ReflectionHelper.GetClassesOfNamespace(
                typeof(AppUser).Namespace, assembly, includeSubns: true);
            var modelTypes = ReflectionHelper.GetClassesOfNamespace(
                typeof(NamespaceModel).Namespace, assembly, includeSubns: true);

            _entityTypes = entityTypes.Concat(modelTypes).ToHashSet();
        }
    }
}
