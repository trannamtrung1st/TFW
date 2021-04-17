using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
