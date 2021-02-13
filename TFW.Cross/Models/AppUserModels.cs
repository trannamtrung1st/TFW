using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models
{
    public class DynamicQueryAppUserModel : BaseQueryModel
    {
        // filter
        public string Id { get; set; }
        public string UserName { get; set; }
    }

    public class AppUserResponseModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
    }
}
