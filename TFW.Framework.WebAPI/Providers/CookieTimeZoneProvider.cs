using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.i18n.Helpers;

namespace TFW.Framework.WebAPI.Providers
{
    public class CookieTimeZoneProviderOptions
    {
        public const string DefaultCookieName = "_tz";

        private string _cookieName = DefaultCookieName;
        public string CookieName
        {
            get => _cookieName; set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _cookieName = value;
            }
        }
    }

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
