using Newtonsoft.Json;

namespace TFW.Cross.Models.AppRole
{
    public class RoleSimpleModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
