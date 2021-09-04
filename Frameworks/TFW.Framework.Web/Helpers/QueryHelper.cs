using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace TFW.Framework.Web.Helpers
{
    public static class QueryHelper
    {
        public static IDictionary<string, StringValues> ParseNullableQueryString(string queryString)
        {
            return QueryHelpers.ParseNullableQuery(queryString);
        }
    }
}
