using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class EnumHelper
    {
        public static string Description(this Enum enumVal, bool fromDisplayAttribute = true)
        {
            var enumType = enumVal.GetType();
            var name = Enum.GetName(enumType, enumVal);
            string desc;

            if (fromDisplayAttribute)
                desc = enumType.GetField(name).GetCustomAttributes(false)
                    .OfType<DisplayAttribute>().SingleOrDefault()?.Description;
            else desc = enumType.GetField(name).GetCustomAttributes(false)
                .OfType<DescriptionAttribute>().SingleOrDefault()?.Description;

            return desc;
        }

        public static string DisplayName(this Enum enumVal, bool fromDisplayAttribute = true)
        {
            var enumType = enumVal.GetType();
            var name = Enum.GetName(enumType, enumVal);
            string displayName;

            if (fromDisplayAttribute)
                displayName = enumType.GetField(name).GetCustomAttributes(false)
                    .OfType<DisplayAttribute>().SingleOrDefault()?.Name;
            else displayName = enumType.GetField(name).GetCustomAttributes(false)
                .OfType<DisplayNameAttribute>().SingleOrDefault()?.DisplayName;

            return displayName;
        }

        public static DisplayAttribute Display(this Enum enumVal)
        {
            var enumType = enumVal.GetType();
            var name = Enum.GetName(enumType, enumVal);
            var displayNameAttr = enumType.GetField(name).GetCustomAttributes(false)
                .OfType<DisplayAttribute>().SingleOrDefault();

            return displayNameAttr;
        }
    }
}
