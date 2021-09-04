using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class AppUserSimpleModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}
