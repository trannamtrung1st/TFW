using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Text;

namespace TFW.Cross
{
    public static class GlobalResources
    {
        public static IDynamicLinkCustomTypeProvider CustomTypeProvider { get; set; }
        public static IEnumerable<Assembly> TempAssemblyList { get; set; }
    }
}
