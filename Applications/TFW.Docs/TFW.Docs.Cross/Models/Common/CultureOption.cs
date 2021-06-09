using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Models.Common
{
    public class CultureOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
