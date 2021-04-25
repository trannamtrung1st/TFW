using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Framework.Web.Extensions;

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
            queryBuilder.AddIfNotNull(nameof(countTotal), countTotal.ToString())
                .AddIfNotNull(nameof(page), page.ToString())
                .AddIfNotNull(nameof(pageLimit), pageLimit.ToString())
                .AddIfNotNull(nameof(sortBy), _sortByArr ?? new string[0])
                .AddIfNotNull(nameof(fields), _fieldsArr ?? new string[0]);
            return queryBuilder;
        }
    }
}
