using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TFW.Framework.Common.Extensions;

namespace TFW.Framework.Common.Helpers
{
    public static class EnumHelper
    {
        public static (int, DisplayAttribute)[] ToDisplayList()
        {
            var type = typeof(Type);
            var enumType = typeof(Enum);
            var enums = type.GetFields().Where(f => f.FieldType.IsSubclassOf(enumType));

            return enums.Select(o => ((int)o.GetRawConstantValue(), o.FieldType.Display(o.Name))).ToArray();
        }

        public static (int, string)[] ToDisplayNameList()
        {
            var type = typeof(Type);
            var enumType = typeof(Enum);
            var enums = type.GetFields().Where(f => f.FieldType.IsSubclassOf(enumType));

            return enums.Select(o => ((int)o.GetRawConstantValue(), o.FieldType.DisplayName(o.Name))).ToArray();
        }

        public static IEnumerable<Type> GetValues<Type>()
        {
            var type = typeof(Type);
            var enumType = typeof(Enum);
            var enums = type.GetFields().Where(f => f.FieldType.IsSubclassOf(enumType));
            return enums.Select(f => (Type)f.GetValue(null));
        }
    }
}
