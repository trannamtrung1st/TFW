using System;

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
