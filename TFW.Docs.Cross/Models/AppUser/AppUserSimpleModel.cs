using Newtonsoft.Json;
using System;
using System.Text;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class AppUserSimpleModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}
