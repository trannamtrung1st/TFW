using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class ArrayHelper
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static string ToCommaString(this IEnumerable<object> arr)
        {
            return string.Join(',', arr);
        }
    }
}
