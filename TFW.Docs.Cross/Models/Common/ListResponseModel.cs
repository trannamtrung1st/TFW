using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.Cross.Models.Common
{
    public class ListResponseModel<T>
    {
        [JsonProperty("list")]
        public IEnumerable<T> List { get; set; }

        [JsonProperty("totalCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalCount { get; set; }
    }
}
