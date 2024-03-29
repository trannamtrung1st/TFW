﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TFW.Docs.AppAdmin.Models;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Setting;

namespace TFW.Docs.AppAdmin
{
    internal static class StartupConfig
    {
        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services.ConfigureSettings(configuration)
                .ConfigureInternationalization();
        }

        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<WebAppSettings>(configuration.GetSection(nameof(WebAppSettings)))
                .Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        }

        public static IServiceCollection ConfigureInternationalization(this IServiceCollection services)
        {
            return services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = Settings.Get<AppSettings>().SupportedCultureNames.ToArray();
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
                options.FallBackToParentCultures = true;
                options.FallBackToParentUICultures = true;
                //options.RequestCultureProviders = ...
            });
        }

        public static IServiceCollection AddWebFrameworks(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = ConfigConsts.i18n.ResourcesPath);

            services.AddRazorPages(options =>
            {
                //options.Conventions.AddPageRoute();
            })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            return services;
        }
    }
}
