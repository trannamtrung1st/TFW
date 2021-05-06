using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.i18n.Helpers;
using TFW.Framework.Web.Options;
using TFW.Framework.Web.Binding;
using TFW.Framework.Web.Middlewares;
using TFW.Framework.Web.Providers;
using TFW.Framework.Web.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace TFW.Framework.Web
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddLogicGroupAuthorizationHandler(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, LogicGroupAuthorizationHandler>();
        }

        public static IServiceCollection AddAuthUserAuthorizationHandler(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthUserAuthorizationHandler>();
        }

        public static IServiceCollection AddDynamicAuthorizationPolicyProvider(this IServiceCollection services,
            Action<DynamicAuthorizationPolicyProviderOptions> configAction)
        {
            return services.Configure(configAction)
                .AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
        }

        public static IServiceCollection ConfigureFrameworkOptions(this IServiceCollection services,
            FrameworkOptionsBuilder builder)
        {
            var original = builder.Build();

            return services.Configure<FrameworkOptions>(opt => opt.CopyFrom(original));
        }

        public static IServiceCollection AddRequestFeatureMiddleware(this IServiceCollection services)
        {
            return services.AddScoped<RequestFeatureMiddleware>();
        }

        public static IServiceCollection AddRequestTimeZoneMiddleware(this IServiceCollection services)
        {
            return services.AddScoped<RequestTimeZoneMiddleware>();
        }

        public static IServiceCollection ConfigureRequestTimeZoneDefault(this IServiceCollection services,
            Action<RequestTimeZoneOptions> requestTimeZoneExtraConfig = null)
        {
            return services.Configure<RequestTimeZoneOptions>(opt =>
            {
                // default
                opt.AllowFallback = true;
                opt.AllowOverrideFallback = true;
                opt.ApplyCurrentTimeZoneToResponseHeaders = true;
                opt.DefaultRequestTimeZone = TimeZoneInfo.Local;
                opt.ResponseHeaderName = RequestTimeZoneOptions.DefaultResponseHeaderName;
                opt.OverrideFallbackCookieName = RequestTimeZoneOptions.DefaultOverrideFallbackCookieName;
                opt.OverrideFallbackHeaderName = RequestTimeZoneOptions.DefaultOverrideFallbackHeaderName;
                opt.OverrideFallbackQueryKey = RequestTimeZoneOptions.DefaultOverrideFallbackQueryKey;
                opt.SupportedTimeZones = TimeZoneHelper.GetAllTimeZones().ToList();

                requestTimeZoneExtraConfig?.Invoke(opt);
            });
        }

        public static IServiceCollection AddTimeZoneAwaredDateTimeModelBinder(this IServiceCollection services)
        {
            return services.AddSingleton(new TimeZoneAwaredDateTimeModelBinder());
        }
    }
}
