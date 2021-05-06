using Newtonsoft.Json;

namespace TFW.Docs.Cross.Models.Setting
{
    public class ChangeSmtpOptionModel
    {
        public static readonly int[] UserNameLength = new[] { 5, 100 };
        public static readonly int[] PasswordLength = new[] { 6, 100 };

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
