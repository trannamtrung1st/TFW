using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TFW.Framework.i18n.Helpers;
using TFW.Framework.Web.Options;
using TFW.Framework.Web.Bindings;
using TFW.Framework.Web.Middlewares;
using TFW.Framework.Web.Providers;
using Microsoft.Extensions.Options;
using TFW.Framework.Web.Handlers;
using Microsoft.AspNetCore.Authorization;
using TFW.Framework.Web.Requirements;

namespace TFW.Framework.Web
{
    public static class ConfigHelper
    {
        public static AuthorizationOptions AddLogicGroup(this AuthorizationOptions opt)
        {
            opt.AddPolicy(LogicGroupRequirement.PolicyName, policy => policy.AddRequirements(new LogicGroupRequirement()));

            return opt;
        }

        public static IServiceCollection AddLogicGroupAuthorizationHandler(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, LogicGroupAuthorizationHandler>();
        }

        public static IServiceCollection AddAuthUserAuthorizationHandler(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, AuthUserAuthorizationHandler>();
        }

        public static DynamicAuthorizationPolicyProviderOptions ConfigureAuthUserDynamicPolicy(
            this DynamicAuthorizationPolicyProviderOptions opt, string policyName)
        {
            opt.Providers[policyName] = (paramList, builder) =>
            {
                var role = string.IsNullOrEmpty(paramList[0]) ? null : paramList[0];

                if (paramList.Length == 1)
                    builder.AddRequirements(new AuthUserRequirement(role));
                else if (paramList.Length == 2)
                    builder.AddRequirements(new AuthUserRequirement(role, paramList[1]));
            };

            return opt;
        }

        public static IServiceCollection AddDynamicAuthorizationPolicyProvider(this IServiceCollection services,
            Action<DynamicAuthorizationPolicyProviderOptions> configAction)
        {
            return services.Configure(configAction)
                .AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
        }

        public static IServiceCollection ConfigureFrameworkOptions(this IServiceCollection services,
            FrameworkOptionsConfigurator configurator)
        {
            var original = configurator.Build();

            return services.Configure<FrameworkOptions>(opt => opt.CopyFrom(original));
        }

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

        public static IApplicationBuilder RegisterOptionsChangeHandlers(this IApplicationBuilder app, params Type[] optionTypes)
        {
            var provider = app.ApplicationServices;

            foreach (var optType in optionTypes)
            {
                var optMonitorType = typeof(IOptionsMonitor<>).MakeGenericType(optType);
                var optMonitor = provider.GetRequiredService(optMonitorType);

                var paramType = typeof(Action<,>).MakeGenericType(optType, typeof(string));
                var onChangeMethod = optMonitorType.GetMethod(nameof(IOptionsMonitor<object>.OnChange), new[] { paramType });

                var handlerType = typeof(IOptionsChangeHandler<>).MakeGenericType(optType);
                var handler = provider.GetRequiredService(handlerType);
                var handlerProp = handlerType.GetProperty(nameof(IOptionsChangeHandler<object>.OnChangeAction));
                var onChangeAction = handlerProp.GetGetMethod().Invoke(handler, null);

                onChangeMethod.Invoke(optMonitor, new[] { onChangeAction });
            }

            return app;
        }
    }
}
