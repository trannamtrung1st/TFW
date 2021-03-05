using Newtonsoft.Json;

namespace TFW.Cross.Models.Exceptions
{
    public class OAuthErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        [JsonProperty("error_uri")]
        public string ErrorUri { get; set; }
    }
}
