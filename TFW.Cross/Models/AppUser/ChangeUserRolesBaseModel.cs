using Newtonsoft.Json;
using System.Collections.Generic;

namespace TFW.Cross.Models.AppUser
{
    public class ChangeUserRolesBaseModel
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}
