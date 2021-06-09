using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.Schema;
using TFW.Framework.DI.Attributes;

namespace TFW.Docs.Cross
{
    [SingletonService]
    [SingletonService(ServiceType = typeof(EntitySchema))]
    public class AppEntitySchema : EntitySchema
    {
        private IDictionary<string, SchemaPropertyInfo> _schema;
        public override IDictionary<string, SchemaPropertyInfo> Schema => _schema;

        public void InitSchema(IDictionary<string, SchemaPropertyInfo> schema)
        {
            if (_schema != null) throw new InvalidOperationException("Already initialized");
            _schema = schema;
        }
    }
}
