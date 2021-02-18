using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Common;

namespace TFW.Cross.Models.Common
{
    public abstract class PagingQueryModel
    {
        public int Page { get; set; } = 1;
        public int PageLimit { get; set; } = QueryConsts.DefaultPageLimit;
    }

    public abstract class BaseDynamicQueryModel : PagingQueryModel
    {
        // projection
        protected string defaultField;

        protected string[] fields;
        public string[] Fields
        {
            get
            {
                if (fields.IsNullOrEmpty() && defaultField != null)
                    fields = new[] { defaultField };

                return fields;
            }
            set
            {
                fields = value;
            }
        }

        // sorting
        public string[] SortBy { get; set; }

        // options
        public bool CountTotal { get; set; }
    }
}
