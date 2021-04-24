using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class AppUserBaseModel : AppUserSimpleModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
