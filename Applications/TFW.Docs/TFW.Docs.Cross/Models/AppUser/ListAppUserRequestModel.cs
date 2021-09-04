using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Web.Extensions;
using AU = TFW.Docs.Cross.Entities.AppUserEntity;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class ListAppUserRequestModel : BaseListRequestModel
    {
        public static new class Parameters
        {
            public const string Ids = "ids";
            public const string UserName = "uname";
            public const string RegisteredFromDate = "rFromDate";
            public const string RegisteredToDate = "rToDate";
        }

        protected override string[] DefaultFields { get; } = new[] { FieldInfo };

        /// <summary>
        /// Ids
        /// </summary>
        [FromQuery(Name = Parameters.Ids)]
        public IEnumerable<int> Ids { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [FromQuery(Name = Parameters.UserName)]
        public string UserName { get; set; }

        /// <summary>
        /// Registered from date
        /// </summary>
        [FromQuery(Name = Parameters.RegisteredFromDate)]
        public DateTimeOffset? RegisteredFromDate { get; set; }

        /// <summary>
        /// Registered to date
        /// </summary>
        [FromQuery(Name = Parameters.RegisteredToDate)]
        public DateTimeOffset? RegisteredToDate { get; set; }

        public override QueryBuilder BuildQuery()
        {
            var builder = base.BuildQuery();
            builder.AddIfNotNull(Parameters.Ids, Ids?.Select(o => o.ToString()).ToArray())
                .AddIfNotNull(Parameters.UserName, UserName)
                .AddIfNotNull(Parameters.RegisteredFromDate, RegisteredFromDate?.ToString("o"))
                .AddIfNotNull(Parameters.RegisteredToDate, RegisteredToDate?.ToString("o"));
            return builder;
        }

        #region Sorting constants
        public const string SortByUsername = "uname";
        public const string DefaultSortBy = "a" + SortByUsername;

        public static readonly IEnumerable<string> SortOptions = new[] { SortByUsername };
        #endregion

        #region Projection constants
        public const string FieldInfo = "info";

        public static readonly IReadOnlyDictionary<string, string> Projections =
            new Dictionary<string, string>()
            {
                {
                    FieldInfo, $"{nameof(AU.Id)},{nameof(AU.UserName)},{nameof(AU.Email)}," +
                    $"{nameof(AU.FullName)},{nameof(AU.CreatedTime)}"
                },
            };
        #endregion
    }
}
