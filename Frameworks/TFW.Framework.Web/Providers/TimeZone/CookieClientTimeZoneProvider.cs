using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TFW.Framework.i18n.Helpers;
using TFW.Framework.Web.Options;

namespace TFW.Framework.Web.Providers
{
    public class CookieClientTimeZoneProvider : IRequestTimeZoneProvider
    {
        public Task<TimeZoneInfo> DetermineRequestTimeZoneAsync(HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<CookieClientTimeZoneProviderOptions>>().Value;
            string timeZoneOffset;

            if (!httpContext.Request.Cookies.TryGetValue(options.CookieName, out timeZoneOffset))
                return Task.FromResult<TimeZoneInfo>(null);

            var offset = double.Parse(timeZoneOffset);

            var timeZoneInfo = TimeZoneHelper.GetFirstTimeZoneByUTCOffset(TimeSpan.FromMinutes(offset));

            return Task.FromResult(timeZoneInfo);
        }
    }
}
