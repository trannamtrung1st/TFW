using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.WebApp.Models;

namespace TFW.Docs.WebApp
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
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
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

        public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
        {
            var webAppSettings = Settings.Get<WebAppSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = SecurityConsts.DefaultTokenParameters;
                    //jwtBearerOptions.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = (context) =>
                    //    {
                    //        StringValues values;
                    //        if (!context.Request.Query.TryGetValue("access_token", out values))
                    //            return Task.CompletedTask;
                    //        var token = values.FirstOrDefault();
                    //        context.Token = token;
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });

            return services;
        }

        public static IServiceCollection AddWebFrameworks(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = ConfigConsts.i18n.ResourcesPath);

            services.AddRazorPages(options =>
            {
                #region Admin
                var authorizeAdminFolders = new[] { AppPages.Folders.Root };
                var allowAnonymousAdminPages = new[] { AppPages.Pages.Login };

                foreach (var folder in authorizeAdminFolders)
                    options.Conventions.AuthorizeAreaFolder(WebAppConsts.Admin.AreaName, folder);
                foreach (var page in allowAnonymousAdminPages)
                    options.Conventions.AllowAnonymousToAreaPage(WebAppConsts.Admin.AreaName, page);
                #endregion
            })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            return services;
        }
    }
}
