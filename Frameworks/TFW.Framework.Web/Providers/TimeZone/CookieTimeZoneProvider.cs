using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TFW.Framework.i18n.Helpers;
using TFW.Framework.Web.Options;

namespace TFW.Framework.Web.Providers
{
    public class CookieTimeZoneProvider : IRequestTimeZoneProvider
    {
        public Task<TimeZoneInfo> DetermineRequestTimeZoneAsync(HttpContext httpContext)
        {
            var options = httpContext.RequestServices.GetRequiredService<IOptions<CookieTimeZoneProviderOptions>>().Value;
            string timeZoneId;

            if (!httpContext.Request.Cookies.TryGetValue(options.CookieName, out timeZoneId))
                return Task.FromResult<TimeZoneInfo>(null);

            var timeZoneInfo = TimeZoneHelper.FindById(timeZoneId);

            return Task.FromResult(timeZoneInfo);
        }
    }
}
