using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TFW.Framework.Common.Extensions
{
    public static class EnumExtensions
    {
        public const string AttributeNotFoundMessage = "No attribute of specified type found on this member";

        public static string Description(this Enum enumVal, bool throwOnNotFound = true)
        {
            var enumType = enumVal.GetType();
            var name = Enum.GetName(enumType, enumVal);

            return enumType.Description(name, throwOnNotFound);
        }

        public static string Description(this Type enumType, string name, bool throwOnNotFound = true)
        {
            var desc = enumType.GetField(name).GetCustomAttributes(false)
                .OfType<DescriptionAttribute>().SingleOrDefault();

            if (desc == null && throwOnNotFound) throw new InvalidOperationException(AttributeNotFoundMessage);

            return desc?.Description;
        }

        public static string DisplayName(this Enum enumVal, bool throwOnNotFound = true)
        {
            var enumType = enumVal.GetType();
            var name = Enum.GetName(enumType, enumVal);

            return enumType.DisplayName(name, throwOnNotFound);
        }

        public static string DisplayName(this Type enumType, string enumName, bool throwOnNotFound = true)
        {
            var displayNameAttr = enumType.GetField(enumName).GetCustomAttributes(false)
                .OfType<DisplayNameAttribute>().SingleOrDefault();

            if (displayNameAttr == null && throwOnNotFound) throw new InvalidOperationException(AttributeNotFoundMessage);

            return displayNameAttr?.DisplayName;
        }

        public static DisplayAttribute Display(this Enum enumVal, bool throwOnNotFound = true)
        {
            var enumType = enumVal.GetType();
            var name = Enum.GetName(enumType, enumVal);

            return enumType.Display(name, throwOnNotFound);
        }

        public static DisplayAttribute Display(this Type enumType, string name, bool throwOnNotFound = true)
        {
            var displayAttr = enumType.GetField(name).GetCustomAttributes(false)
                .OfType<DisplayAttribute>().SingleOrDefault();

            if (displayAttr == null && throwOnNotFound) throw new InvalidOperationException(AttributeNotFoundMessage);

            return displayAttr;
        }

        public static string Name(this Enum enumVal)
        {
            return Enum.GetName(enumVal.GetType(), enumVal);
        }

        public static string ToStringF(this Enum enumVal)
        {
            return enumVal.ToString("F");
        }

        public static string ToStringG(this Enum enumVal)
        {
            return enumVal.ToString("G");
        }
    }
}
