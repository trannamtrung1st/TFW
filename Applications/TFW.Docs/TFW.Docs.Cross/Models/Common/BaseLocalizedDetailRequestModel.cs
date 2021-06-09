using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using TFW.Framework.Web.Extensions;

namespace TFW.Docs.Cross.Models.Common
{
    public abstract class BaseLocalizedDetailRequestModel
    {
        public static class Parameters
        {
            public const string Lang = "lang";
            public const string Region = "region";
            public const string Fallback = "fb";
        }

        /// <summary>
        /// Lang
        /// </summary>
        [FromQuery(Name = Parameters.Lang)]
        public string Lang { get; set; }

        /// <summary>
        /// Lang
        /// </summary>
        [FromQuery(Name = Parameters.Region)]
        public string Region { get; set; }

        /// <summary>
        /// Fallback
        /// </summary>
        [FromQuery(Name = Parameters.Fallback)]
        public bool Fallback { get; set; } = true;

        public virtual QueryBuilder BuildQuery()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.AddIfNotNull(Parameters.Lang, Lang)
                .AddIfNotNull(Parameters.Region, Region)
                .AddIfNotNull(Parameters.Fallback, $"{Fallback}");
            return queryBuilder;
        }
    }
}
