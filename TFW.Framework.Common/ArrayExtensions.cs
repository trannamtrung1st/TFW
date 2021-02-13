using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Common
{
    public static class ArrayExtensions
    {
        public static string ToCommaString(this object[] arr)
        {
            return string.Join(',', arr);
        }
    }
}
