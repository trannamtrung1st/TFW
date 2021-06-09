using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using TFW.Framework.Web.Extensions;

namespace TFW.Docs.Cross.Models.Common
{
    public abstract class BaseLocalizedListRequestModel : BaseListRequestModel
    {
        public static new class Parameters
        {
            public const string Lang = "lang";
            public const string Region = "region";
        }

        /// <summary>
        /// Lang
        /// </summary>
        [FromQuery(Name = Parameters.Lang)]
        public string Lang { get; set; }

        /// <summary>
        /// Region
        /// </summary>
        [FromQuery(Name = Parameters.Region)]
        public string Region { get; set; }

        public override QueryBuilder BuildQuery()
        {
            var queryBuilder = base.BuildQuery();
            queryBuilder.AddIfNotNull(Parameters.Lang, Lang)
                .AddIfNotNull(Parameters.Region, Region);
            return queryBuilder;
        }
    }
}
