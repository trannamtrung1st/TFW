using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.Common
{
    public class CultureOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
