using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFW.Docs.Cross.Models.Common
{
    public abstract class BaseGetListRequestModel
    {
        public bool countTotal { get; set; }
        public int page { get; set; } = QueryConsts.DefaultPage;
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

        public virtual QueryBuilder BuildQuery()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add(nameof(countTotal), countTotal.ToString());
            queryBuilder.Add(nameof(page), page.ToString());
            queryBuilder.Add(nameof(pageLimit), pageLimit.ToString());
            queryBuilder.Add(nameof(sortBy), _sortByArr);
            queryBuilder.Add(nameof(fields), _fieldsArr);
            return queryBuilder;
        }
    }
}
