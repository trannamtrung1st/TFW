using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Text;
using TFW.Cross.Entities;
using TFW.Framework.Common;
using TFW.Framework.DI;

namespace TFW.Cross.Commons
{
    [SingletonService(ServiceType = typeof(IDynamicLinkCustomTypeProvider))]
    public class DynamicLinqEntityTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        private static HashSet<Type> _entityTypes;
        static DynamicLinqEntityTypeProvider()
        {
            _entityTypes = ReflectionHelper.GetClassesOfNamespace(typeof(AppUser).Namespace).ToHashSet();
        }

        public override HashSet<Type> GetCustomTypes() => _entityTypes;
    }
}
