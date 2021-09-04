using Newtonsoft.Json;

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
