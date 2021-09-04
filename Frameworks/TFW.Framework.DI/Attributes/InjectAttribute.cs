using System;

namespace TFW.Framework.DI.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public bool Required { get; set; } = true;
    }
}
