using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TFW.Framework.i18n.Helpers
{
    public static class CultureHelper
    {
        public static CultureInfo[] GetCultures(CultureTypes type)
        {
            return CultureInfo.GetCultures(type);
        }
    }
}
