using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models.AppRole
{
    public class GetListRolesResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
