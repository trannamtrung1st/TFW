using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Web.Extensions;

namespace TFW.Docs.Cross.Models.Common
{
    public abstract class BaseGetListRequestModel
    {
        public static class Parameters
        {
            public const string CountTotal = "cTotal";
            public const string Page = "p";
            public const string PageLimit = "pLimit";
            public const string SortBy = "sort";
            public const string Fields = "f";
        }

        protected abstract string[] DefaultFields { get; }

        /// <summary>
        /// Count total
        /// </summary>
        [FromQuery(Name = Parameters.CountTotal)]
        public bool CountTotal { get; set; }

        /// <summary>
        /// Page
        /// </summary>
        [FromQuery(Name = Parameters.Page)]
        public int Page { get; set; } = QueryConsts.DefaultPage;
        
        /// <summary>
        /// Page limit
        /// </summary>
        [FromQuery(Name = Parameters.PageLimit)]
        public int PageLimit { get; set; } = QueryConsts.DefaultPageLimit;

        #region Sorting
        private string _sortBy;

        /// <summary>
        /// Sort by
        /// </summary>
        [FromQuery(Name = Parameters.SortBy)]
        public string SortBy
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
        #endregion

        #region Projection
        private string _fields;

        /// <summary>
        /// Fields
        /// </summary>
        [FromQuery(Name = Parameters.Fields)]
        public string Fields
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
                    _fieldsArr = _fieldsArr.IsNullOrEmpty() ? DefaultFields : _fieldsArr;
                }
            }
        }

        private string[] _fieldsArr;
        public string[] GetFieldsArr()
        {
            return _fieldsArr ?? DefaultFields;
        }
        #endregion

        public virtual QueryBuilder BuildQuery()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.AddIfNotNull(Parameters.CountTotal, $"{CountTotal}")
                .AddIfNotNull(Parameters.Page, $"{Page}")
                .AddIfNotNull(Parameters.PageLimit, $"{PageLimit}")
                .AddIfNotNull(Parameters.SortBy, _sortByArr ?? new string[0])
                .AddIfNotNull(Parameters.Fields, _fieldsArr ?? new string[0]);
            return queryBuilder;
        }
    }
}
