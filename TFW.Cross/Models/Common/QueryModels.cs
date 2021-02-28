using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Common.Helpers;

namespace TFW.Cross.Models.Common
{
    public abstract class PagingQueryModel
    {
        public int Page { get; set; } = QueryConsts.DefaultPage;
        public int PageLimit { get; set; } = QueryConsts.DefaultPageLimit;
    }

    public abstract class BaseDynamicQueryModel : PagingQueryModel
    {
        // projection
        protected abstract string[] DefaultFields { get; }

        protected string[] fields;
        public string[] Fields
        {
            get
            {
                return fields ?? DefaultFields;
            }
            set
            {
                fields = value.IsNullOrEmpty() ? DefaultFields : value;
            }
        }

        // sorting
        public string[] SortBy { get; set; }

        // options
        public bool CountTotal { get; set; }
    }
}
