using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TFW.Framework.Common
{
    public static class EnumExtensions
    {
        public static string Description(this Enum enumVal)
        {
            var enumType = enumVal.GetType();
            var name = Enum.GetName(enumType, enumVal);
            var displayNameAttr = enumType.GetField(name).GetCustomAttributes(false)
                .OfType<DescriptionAttribute>().SingleOrDefault();
            return displayNameAttr?.Description;
        }
    }
}
