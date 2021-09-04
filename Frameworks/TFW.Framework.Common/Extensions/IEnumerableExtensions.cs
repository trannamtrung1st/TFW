using System;
using System.Collections.Generic;
using System.Linq;

namespace TFW.Framework.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T[] SubSet<T>(this T[] arr, int fromIdx, int length)
        {
            var newArr = new T[length];

            Array.Copy(arr, fromIdx, newArr, 0, length);

            return newArr;
        }

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
