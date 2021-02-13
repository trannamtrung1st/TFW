using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models
{
    public abstract class PagingQueryModel
    {
        public int Page { get; set; } = 1;
        public int PageLimit { get; set; } = QueryConsts.DefaultPageLimit;
    }

    public abstract class BaseQueryModel : PagingQueryModel
    {
        // projection
        public string[] Fields { get; set; }

        // sorting
        public string[] SortBy { get; set; }

        // options
        public bool CountTotal { get; set; }
    }

    public class GetListResponseModel<T>
    {
        [JsonProperty("list")]
        public T[] List { get; set; }
        [JsonProperty("totalCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalCount { get; set; }
    }
}
