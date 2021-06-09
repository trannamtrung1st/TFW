using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Configuration
{
    public static class ConfigFiles
    {
        public static class AppSettings
        {
            public const string Default = "appsettings.json";
            public const string DefaultEnv = "appsettings." + EnvPlaceholder + ".json";
            public const string EnvPlaceholder = "{env}";
        }
    }
}
