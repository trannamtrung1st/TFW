using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.i18n.Helpers;
using TFW.Framework.Web.Options;

namespace TFW.Framework.Web.Providers
{
    public class HeaderClientTimeZoneProvider : IRequestTimeZoneProvider
    {
        public Task<TimeZoneInfo> DetermineRequestTimeZoneAsync(HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<HeaderClientTimeZoneProviderOptions>>().Value;
            StringValues timeZoneOffsets;

            if (!httpContext.Request.Headers.TryGetValue(options.HeaderName, out timeZoneOffsets))
                return Task.FromResult<TimeZoneInfo>(null);

            var offset = double.Parse(timeZoneOffsets.First());

            var timeZoneInfo = TimeZoneHelper.GetFirstTimeZoneByUTCOffset(TimeSpan.FromMinutes(offset));

            return Task.FromResult(timeZoneInfo);
        }
    }
}
