using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TFW.Cross;
using TFW.Cross.Models.Setting;
using TFW.Cross.Providers;
using TFW.Cross.Requirements;
using TFW.Data.Providers;
using TFW.Framework.Web;
using TFW.Framework.Web.Options;
using TFW.Framework.Web.Requirements;
using TFW.WebAPI.Handlers;
using TFW.WebAPI.Middlewares;
using TFW.WebAPI.Models;
using TFW.WebAPI.Providers;

namespace TFW.WebAPI
{
    public static class ConfigHelper
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policy.Name.BackdoorUser, policy => policy.RequireUserName("user0707"));

                opt.AddPolicy(Policy.Name.GuestRestriction, policy => policy.AddRequirements(
                    new GuestRestrictionRequirement(mustBefore: Policy.GuestRestrictionMustBefore)));

                opt.AddPolicy(Policy.Name.AdminOrOwner, policy => policy.RequireAuthenticatedUser()
                    .AddRequirements(new AdminOrOwnerRequirement()));

                opt.AddLogicGroup();
            });

            services.AddSingleton<IAuthorizationHandler, GuestRestrictionAuthorizationHandler>();

            services.AddSingleton<IAuthorizationHandler, AdminOrOwnerAuthorizationHandler>();

            services.AddAuthUserAuthorizationHandler();

            services.AddLogicGroupAuthorizationHandler();

            services.AddDynamicAuthorizationPolicyProvider(opt =>
            {
                opt.Seperator = DynamicAuthorizationPolicyProviderOptions.DefaultSeperator;
                opt.DefaultSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };

                opt.Providers[Policy.Name.AdminOrOwner] = (paramList, builder) =>
                    builder.AddRequirements(new AdminOrOwnerRequirement(paramList[0]));

                opt.Providers[Policy.Name.GuestRestriction] = (paramList, builder) =>
                    builder.AddRequirements(new GuestRestrictionRequirement(
                        mustBefore: Policy.GuestRestrictionMustBefore, paramList[0]));

                opt.ConfigureAuthUserDynamicPolicy(Policy.Name.AuthUser);
            });

            return services;
        }

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
