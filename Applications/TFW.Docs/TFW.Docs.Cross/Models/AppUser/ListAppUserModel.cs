using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class ListAppUserModel : AppUserBaseModel
    {
        [JsonProperty("createdTime")]
        public DateTimeOffset CreatedTime { get; set; }
    }
}
