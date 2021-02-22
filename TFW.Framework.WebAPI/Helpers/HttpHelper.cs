using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.WebAPI.Helpers
{
    public static class HttpHelper
    {
        public static T GetRequiredService<T>(this HttpContext context)
        {
            return context.RequestServices.GetRequiredService<T>();
        }

        public static T GetItem<T>(this HttpContext context, string key)
        {
            if (context?.Items.ContainsKey(key) != true)
                throw new KeyNotFoundException(key);

            return (T)context.Items[key];
        }

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

        public static IDictionary<string, StringValues> ParseNullableQueryString(string queryString)
        {
            return QueryHelpers.ParseNullableQuery(queryString);
        }
    }
}
