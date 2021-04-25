using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Text;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Web.Extensions;

namespace TFW.Docs.Cross.Models.AppUser
{
    public class GetListAppUsersRequestModel : BaseGetListRequestModel
    {
        public int? id { get; set; }
        public string userName { get; set; }
        public string searchTerm { get; set; }

        public override QueryBuilder BuildQuery()
        {
            var builder = base.BuildQuery();
            builder.AddIfNotNull(nameof(id), id?.ToString())
                .AddIfNotNull(nameof(userName), userName)
                .AddIfNotNull(nameof(searchTerm), searchTerm);
            return builder;
        }
    }
}
