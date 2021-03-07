using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TFW.Cross.Models.Setting;
using TFW.Cross.Providers;
using TFW.Data.Providers;
using TFW.WebAPI.Middlewares;
using TFW.WebAPI.Models;
using TFW.WebAPI.Providers;

namespace TFW.WebAPI
{
    public static class ConfigHelper
    {
        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services, IConfiguration config)
        {
            return services.Configure<AppSettings>(config.GetSection(nameof(AppSettings)))
                .Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)))
                .Configure<ApiSettings>(config.GetSection(nameof(ApiSettings)));
        }

        public static IServiceCollection AddHttpUnitOfWorkProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IUnitOfWorkProvider, HttpUnitOfWorkProvider>();
        }

        public static IServiceCollection AddHttpBusinessContextProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IBusinessContextProvider, HttpBusinessContextProvider>();
        }

        public static IApplicationBuilder UseRequestDataExtraction(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestDataExtractionMiddleware>();
        }
    }
}
