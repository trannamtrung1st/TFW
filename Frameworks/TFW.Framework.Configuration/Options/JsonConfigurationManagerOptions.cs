using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Configuration.Options
{
    public class JsonConfigurationManagerOptions
    {
        public string ConfigFilePath { get; set; }
        public string FallbackFilePath { get; set; }
    }
}
