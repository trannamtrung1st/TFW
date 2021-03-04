using Newtonsoft.Json;
using System.Collections.Generic;

namespace TFW.Cross.Models.AppUser
{
    public class GetListAppUsersResponseModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("notes")]
        public IEnumerable<NoteResponseModel> Notes { get; set; }
    }
}
