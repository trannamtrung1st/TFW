using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Extensions
{
    public static class QueryBuilderExtensions
    {
        public static QueryBuilder AddIfNotNull(this QueryBuilder builder, string key, string value)
        {
            if (value != null)
                builder.Add(key, value);
            return builder;
        }

        public static QueryBuilder AddIfNotNull(this QueryBuilder builder, string key, IEnumerable<string> values)
        {
            if (values != null)
                builder.Add(key, values);
            return builder;
        }
    }
}
