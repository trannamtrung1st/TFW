using Microsoft.AspNetCore.Authentication.Cookies;
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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.Cookie.HttpOnly = true;
                   options.AccessDeniedPath = Routing.Admin.AccessDenied;
                   options.ExpireTimeSpan = TimeSpan.FromHours(webAppSettings.CookiePersistenceHours);
                   options.LoginPath = Routing.Admin.Login;
                   options.LogoutPath = Routing.Admin.Logout;
                   options.ReturnUrlParameter = WebAppConsts.Admin.ReturnUrlParameter;
                   options.SlidingExpiration = true;
                   //options.Events.OnValidatePrincipal = async (c) =>
                   //{
                   //    await SecurityStampValidator.ValidatePrincipalAsync(c);
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
                var authorizeAdminFolders = new[] { WebAppConsts.Admin.Folder_Root };
                var allowAnonymousAdminPages = new[] { WebAppConsts.Admin.Page_Login };

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
