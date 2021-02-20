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
    public class HeaderTimeZoneProviderOptions
    {
        public const string DefaultHeaderName = "Content-TZ";

        private string _headerName = DefaultHeaderName;
        public string HeaderName
        {
            get => _headerName; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _headerName = value;
            }
        }
    }

    public class HeaderTimeZoneProvider : IRequestTimeZoneProvider
    {
        public Task<TimeZoneInfo> DetermineRequestTimeZoneAsync(HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<HeaderTimeZoneProviderOptions>>().Value;
            StringValues timeZoneIds;

            if (!httpContext.Request.Headers.TryGetValue(options.HeaderName, out timeZoneIds))
                return Task.FromResult<TimeZoneInfo>(null);

            var timeZoneInfo = TimeZoneHelper.FindById(timeZoneIds.FirstOrDefault());

            return Task.FromResult(timeZoneInfo);
        }
    }
}
