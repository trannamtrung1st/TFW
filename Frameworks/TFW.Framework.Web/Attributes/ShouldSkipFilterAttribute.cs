using System;

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
