using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.i18n;
using TFW.Framework.Web.Options;

namespace TFW.Framework.Web.Middlewares
{
    public class RequestTimeZoneMiddleware : ScopedSafeMiddleware
    {
        private readonly RequestTimeZoneOptions _options;

        public RequestTimeZoneMiddleware(IOptions<RequestTimeZoneOptions> options)
        {
            _options = options.Value;
        }

        protected override async Task ForwardInvokeAsync(HttpContext context)
        {
            TimeZoneInfo requestTimeZone = null;
            var allowFallbackTask = IsFallbackAllowedAsync(context);

            foreach (var provider in _options.Providers)
            {
                try
                {
                    requestTimeZone = await provider.DetermineRequestTimeZoneAsync(context);
                }
                catch (TimeZoneNotFoundException e)
                {
                    if (!allowFallbackTask.Result) throw e;
                }

                if (requestTimeZone != null)
                {
                    if (!_options.SupportedTimeZones.Contains(requestTimeZone))
                    {
                        if (!allowFallbackTask.Result)
                            throw new ArgumentException($"Not supported: {requestTimeZone}");
                        else requestTimeZone = null;
                    }
                    else break;
                }
            }

            Time.ThreadTimeZone = requestTimeZone ?? _options.DefaultRequestTimeZone;

            if (_options.ApplyCurrentTimeZoneToResponseHeaders)
            {
                var headerValue = string.Join(';',
                    Time.ThreadTimeZone.Id, Time.ThreadTimeZone.DisplayName,
                    $"{Time.ThreadTimeZone.BaseUtcOffset.TotalMinutes}");
                context.Response.Headers.Add(
                    RequestTimeZoneOptions.TimeZoneResponseHeaderName, headerValue);
            }
        }

        public virtual Task<bool> IsFallbackAllowedAsync(HttpContext httpContext)
        {
            bool? overrideFallback = null;

            if (_options.AllowOverrideFallback)
            {
                StringValues fallbackStrVals; string fallbackStr;
                bool fallback;

                if (httpContext.Request.Query.TryGetValue(_options.OverrideFallbackQueryKey, out fallbackStrVals))
                {
                    if (bool.TryParse(fallbackStrVals.First(), out fallback))
                        overrideFallback = fallback;
                }
                else if (httpContext.Request.Headers.TryGetValue(_options.OverrideFallbackHeaderName, out fallbackStrVals))
                {
                    if (bool.TryParse(fallbackStrVals.First(), out fallback))
                        overrideFallback = fallback;
                }
                else if (httpContext.Request.Cookies.TryGetValue(_options.OverrideFallbackCookieName, out fallbackStr))
                {
                    if (bool.TryParse(fallbackStr, out fallback))
                        overrideFallback = fallback;
                }
            }

            var finalAllowFallback = overrideFallback != false && (overrideFallback == true || _options.AllowFallback);
            return Task.FromResult(finalAllowFallback);
        }

        protected override Task BackwardInvokeAsync(HttpContext context)
        {
            return Task.CompletedTask;
        }
    }
}
