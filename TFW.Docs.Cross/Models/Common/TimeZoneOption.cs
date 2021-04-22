using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Models.Common
{
    public class TimeZoneOption
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("utcOffsetInMinutes")]
        public int UtcOffsetInMinutes { get; set; }
    }
}
