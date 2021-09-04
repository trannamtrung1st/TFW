using Newtonsoft.Json;
using System.Collections.Generic;

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
