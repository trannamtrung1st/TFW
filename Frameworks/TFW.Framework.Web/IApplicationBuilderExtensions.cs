using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using TFW.Framework.Web.Handlers;
using TFW.Framework.Web.Middlewares;

namespace TFW.Framework.Web
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigureHttpContext(this IApplicationBuilder app)
        {
            System.Web.HttpContext.Configure(
                app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());

            return app;
        }

        public static IApplicationBuilder UseRequestFeature(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestFeatureMiddleware>();
        }

        public static IApplicationBuilder UseRequestTimeZone(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestTimeZoneMiddleware>();
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
