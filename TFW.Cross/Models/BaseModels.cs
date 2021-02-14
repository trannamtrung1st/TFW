using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models
{
    public abstract class BaseGetListRequestModel
    {
        public string[] fields { get; set; }
        public string[] sortBy { get; set; }
        public bool countTotal { get; set; }
        public int page { get; set; } = 1;
        public int pageLimit { get; set; } = QueryConsts.DefaultPageLimit;
    }

    public abstract class PagingQueryModel
    {
        public int Page { get; set; } = 1;
        public int PageLimit { get; set; } = QueryConsts.DefaultPageLimit;
    }

    public abstract class BaseDynamicQueryModel : PagingQueryModel
    {
        // projection
        public string[] Fields { get; set; }

        // sorting
        public string[] SortBy { get; set; }

        // options
        public bool CountTotal { get; set; }
    }

}
