using Newtonsoft.Json;

namespace TFW.Cross.Models.AppUser
{
    public class AppUserBaseModel : AppUserSimpleModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
