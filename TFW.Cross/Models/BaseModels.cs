using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Framework.Common;

namespace TFW.Cross.Models
{
    public abstract class BaseGetListRequestModel
    {
        public bool countTotal { get; set; }
        public int page { get; set; } = 1;
        public int pageLimit { get; set; } = QueryConsts.DefaultPageLimit;

        private string _sortBy;
        public string sortBy
        {
            get
            {
                return _sortBy;
            }
            set
            {
                if (value?.Length > 0)
                {
                    _sortBy = value;
                    _sortByArr = value.Split(',').ToArray();
                }
            }
        }

        private string[] _sortByArr;
        public string[] GetSortByArr()
        {
            return _sortByArr;
        }

        private string _fields;
        public string fields
        {
            get
            {
                return _fields;
            }
            set
            {
                if (value?.Length > 0)
                {
                    _fields = value;
                    _fieldsArr = value.Split(',').ToArray();
                }
            }
        }

        private string[] _fieldsArr;
        public string[] GetFieldsArr()
        {
            return _fieldsArr;
        }

    }

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
