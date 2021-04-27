using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Common.Helpers
{
    public static class AppHelper
    {
        public static string GetAppContextBaseDirectory()
        {
            return AppContext.BaseDirectory;
        }
    }
}
