using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Cross.Schema
{
    public abstract class EntitySchema
    {
        public abstract IDictionary<string, SchemaPropertyInfo> Schema { get; }

        public StringPropertyInfo GetString(Type entityType, string propName)
        {
            SchemaPropertyInfo info;

            Schema.TryGetValue($"{entityType.Namespace}.{entityType.Name}.{propName}", out info);

            if (info is StringPropertyInfo strInfo) return strInfo;

            return null;
        }

        public StringPropertyInfo GetString<T>(string propName)
        {
            return GetString(typeof(T), propName);
        }
    }
}
