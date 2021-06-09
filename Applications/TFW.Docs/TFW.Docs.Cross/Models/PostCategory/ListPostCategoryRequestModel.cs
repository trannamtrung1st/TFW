using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Web.Extensions;
using PC = TFW.Docs.Cross.Models.PostCategory.ListPostCategoryJoinModel;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class ListPostCategoryRequestModel : BaseLocalizedListRequestModel
    {
        public static new class Parameters
        {
            public const string Ids = "ids";
        }

        protected override string[] DefaultFields { get; } = new[] { FieldInfo };

        /// <summary>
        /// Ids
        /// </summary>
        [FromQuery(Name = Parameters.Ids)]
        public IEnumerable<int> Ids { get; set; }

        public override QueryBuilder BuildQuery()
        {
            var builder = base.BuildQuery();
            builder.AddIfNotNull(Parameters.Ids, Ids?.Select(o => o.ToString()));
            return builder;
        }

        #region Sorting constants
        public const string SortByTitle = "title";
        public const string DefaultSortBy = "a" + SortByTitle;

        public static readonly IEnumerable<string> SortOptions = new[] { SortByTitle };
        #endregion

        #region Projection constants
        public const string FieldInfo = "info";

        public static readonly IReadOnlyDictionary<string, string> Projections =
            new Dictionary<string, string>()
            {
                {
                    FieldInfo, $"{nameof(PC.Id)},{nameof(PC.Title)},{nameof(PC.Lang)}," +
                    $"{nameof(PC.Region)},{nameof(PC.Description)},{nameof(PC.CreatedTime)}"
                },
            };
        #endregion
    }
}
