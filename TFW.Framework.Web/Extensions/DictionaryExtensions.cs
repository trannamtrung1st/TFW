using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace TFW.Framework.Web.Helpers
{
    public static class DictionaryExtensions
    {
        public static QueryString ToQueryString(this IDictionary<string, StringValues> map)
        {
            var queryBuilder = new QueryBuilder();

            foreach (var kvp in map)
                queryBuilder.Add(kvp.Key, kvp.Value.ToArray());

            return queryBuilder.ToQueryString();
        }

        public static QueryString ToQueryString(this IDictionary<string, string> map)
        {
            var queryBuilder = new QueryBuilder(map);

            return queryBuilder.ToQueryString();
        }
    }
}
