using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.i18n.Helpers;
using TFW.Framework.WebAPI.Bindings;
using TFW.Framework.WebAPI.Middlewares;
using TFW.Framework.WebAPI.Options;
using TFW.Framework.WebAPI.Providers;

namespace TFW.Framework.WebAPI
{
    public static class ConfigHelper
    {
        public static IApplicationBuilder ConfigureHttpContext(this IApplicationBuilder app)
        {
            System.Web.HttpContext.Configure(
                app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            return app;
        }

        public static IApplicationBuilder UseRequestTimeZone(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestTimeZoneMiddleware>();
        }

        public static IServiceCollection AddRequestTimeZoneMiddleware(this IServiceCollection services)
        {
            return services.AddScoped<RequestTimeZoneMiddleware>();
        }

        public static IServiceCollection ConfigureRequestTimeZoneDefault(this IServiceCollection services,
            Action<RequestTimeZoneOptions> requestTimeZoneExtraConfig = null,
            bool addQuery = true, bool addHeader = true, bool addCookie = true,
            Action<QueryTimeZoneProviderOptions> queryExtraConfig = null,
            Action<HeaderTimeZoneProviderOptions> headerExtraConfig = null,
            Action<CookieTimeZoneProviderOptions> cookieExtraConfig = null)
        {
            if (addQuery)
                services.Configure<QueryTimeZoneProviderOptions>(opt =>
                {
                    // default
                    opt.QueryKey = QueryTimeZoneProviderOptions.DefaultQueryKey;

                    queryExtraConfig?.Invoke(opt);
                });

            if (addHeader)
                services.Configure<HeaderTimeZoneProviderOptions>(opt =>
                {
                    // default
                    opt.HeaderName = HeaderTimeZoneProviderOptions.DefaultHeaderName;

                    headerExtraConfig?.Invoke(opt);
                });

            if (addCookie)
                services.Configure<CookieTimeZoneProviderOptions>(opt =>
                {
                    // default
                    opt.CookieName = CookieTimeZoneProviderOptions.DefaultCookieName;

                    cookieExtraConfig?.Invoke(opt);
                });

            return services.Configure<RequestTimeZoneOptions>(opt =>
            {
                // default
                opt.AllowFallback = false;
                opt.AllowOverrideFallback = true;
                opt.ApplyCurrentTimeZoneToResponseHeaders = true;
                opt.DefaultRequestTimeZone = TimeZoneInfo.Local;
                opt.OverrideFallbackCookieName = RequestTimeZoneOptions.DefaultOverrideFallbackCookieName;
                opt.OverrideFallbackHeaderName = RequestTimeZoneOptions.DefaultOverrideFallbackHeaderName;
                opt.OverrideFallbackQueryKey = RequestTimeZoneOptions.DefaultOverrideFallbackQueryKey;
                opt.SupportedTimeZones = TimeZoneHelper.GetAllTimeZones().ToList();

                requestTimeZoneExtraConfig?.Invoke(opt);

                if (addQuery)
                    opt.AddProvider(new QueryTimeZoneProvider());

                if (addHeader)
                    opt.AddProvider(new HeaderTimeZoneProvider());

                if (addCookie)
                    opt.AddProvider(new CookieTimeZoneProvider());
            });
        }

        public static IServiceCollection AddDefaultDateTimeModelBinder(this IServiceCollection services)
        {
            return services.AddSingleton(new DefaultDateTimeModelBinder());
        }
    }
}
