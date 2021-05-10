using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Web.Extensions;

namespace TFW.Docs.Cross.Models.PostCategory
{
    public class PostCategoryDetailRequestModel : BaseLocalizedDetailRequestModel
    {
        public static new class Parameters
        {
            public const string Id = "id";
        }

        /// <summary>
        /// Id
        /// </summary>
        [FromRoute(Name = "id")]
        public int Id { get; set; }

        public override QueryBuilder BuildQuery()
        {
            var queryBuilder = base.BuildQuery();
            queryBuilder.AddIfNotNull(Parameters.Id, $"{Id}");
            return queryBuilder;
        }
    }
}
