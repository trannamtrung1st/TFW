using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace TFW.Docs.Cross.Models.Common
{
    public class RegionOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        
        [JsonProperty("englishName")]
        public string EnglishName { get; set; }
    }
}
