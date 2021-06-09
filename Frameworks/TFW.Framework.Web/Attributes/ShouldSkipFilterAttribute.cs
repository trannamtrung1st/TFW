using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ShouldSkipFilterAttribute : Attribute
    {
        public Type[] SkippedFilterTypes { get; }

        public ShouldSkipFilterAttribute(params Type[] skippedFilterTypes)
        {
            SkippedFilterTypes = skippedFilterTypes ?? new Type[] { };
        }
    }
}
