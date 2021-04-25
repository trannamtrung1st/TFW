using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Text;
using TFW.Docs.Cross.Models.Common;

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
            builder.Add(nameof(id), id?.ToString());
            builder.Add(nameof(userName), userName);
            builder.Add(nameof(searchTerm), searchTerm);
            return builder;
        }
    }
}
