using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Common
{
    public static class ArrayHelper
    {
        public static bool IsNullOrEmpty(this object[] arr)
        {
            return arr == null || arr.Length == 0;
        }

        public static string ToCommaString(this object[] arr)
        {
            return string.Join(',', arr);
        }
    }
}
