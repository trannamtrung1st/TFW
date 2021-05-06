using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class GetListAppUsersResponseModel : AppUserBaseModel
    {
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }
    }
}
