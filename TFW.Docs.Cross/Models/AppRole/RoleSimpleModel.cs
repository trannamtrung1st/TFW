using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.AppRole
{
    public class RoleSimpleModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
