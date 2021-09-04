using System;

namespace TFW.Framework.Cross.Schema
{
    public class SchemaPropertyInfo
    {
        public Type PropertyType { get; set; }
    }

    public class StringPropertyInfo : SchemaPropertyInfo
    {
        public int? StringLength { get; set; }
        public bool IsFixLength { get; set; }
        public bool IsUnboundLength { get; set; }
    }
}
