using Newtonsoft.Json;
using System;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class ListAppUserModel : AppUserBaseModel
    {
        [JsonProperty("createdTime")]
        public DateTimeOffset CreatedTime { get; set; }
    }
}
