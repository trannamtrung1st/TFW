using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.i18n.Helpers;

namespace TFW.Framework.WebAPI.Providers
{
    public class QueryTimeZoneProviderOptions
    {
        public const string DefaultQueryKey = "tz";

        private string _queryKey = DefaultQueryKey;
        public string QueryKey
        {
            get => _queryKey; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _queryKey = value;
            }
        }
    }

    public class QueryTimeZoneProvider : IRequestTimeZoneProvider
    {
        public Task<TimeZoneInfo> DetermineRequestTimeZoneAsync(HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<QueryTimeZoneProviderOptions>>().Value;
            StringValues timeZoneIds;

            if (!httpContext.Request.Query.TryGetValue(options.QueryKey, out timeZoneIds))
                return Task.FromResult<TimeZoneInfo>(null);

            var timeZoneInfo = TimeZoneHelper.FindById(timeZoneIds.FirstOrDefault());

            return Task.FromResult(timeZoneInfo);
        }
    }
}
