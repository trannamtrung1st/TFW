using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using TFW.Cross;
using TFW.Cross.Models.Setting;
using TFW.Cross.Providers;
using TFW.Cross.Requirements;
using TFW.Data.Core;
using TFW.Data.Providers;
using TFW.Framework.Data;
using TFW.Framework.Data.Options;
using TFW.Framework.Web;
using TFW.Framework.Web.Options;
using TFW.WebAPI.Handlers;
using TFW.WebAPI.Middlewares;
using TFW.WebAPI.Models;
using TFW.WebAPI.Providers;

namespace TFW.WebAPI
{
    public static class ConfigHelper
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, string connStr)
        {
            if (Settings.App.UseDbConnectionPool)
            {
                IDbConnectionPoolManager connectionPoolManager;

                void Pool_RetryAddToPoolError(Exception ex, int tryCount)
                {
                    Log.Error(ex, "Failed {TryCount} time(s) in retrying add DbConnection to pool", tryCount);
                }

                services.AddSqlConnectionPoolManager(out connectionPoolManager, configAction: options =>
                {
                    options.WatchIntervalInMinutes = 1;
                    options.RetryIntervalInSeconds = SqlConnectionPoolManagerOptions.DefaultRetryIntervalInSeconds;
                    //options.WatchIntervalInMinutes = SqlConnectionPoolManagerOptions.DefaultWatchIntervalInMinutes;
                    //options.RetryIntervalInSeconds = SqlConnectionPoolManagerOptions.DefaultRetryIntervalInSeconds;
                }, initAction: async pool =>
                {
                    pool.RetryAddToPoolError += Pool_RetryAddToPoolError;

                    await pool.InitDbConnectionAsync(new SqlConnectionPoolOptions
                    {
                        ConnectionString = connStr,
                        LifetimeInMinutes = 1,
                        //LifetimeInMinutes = SqlConnectionPoolOptions.DefaultLifetimeInMinutes,
                        MaximumConnections = 100,
                        MinimumConnections = 1
                    }, DataConsts.ConnStrKey);
                }).AddDbContext<DataContext>(async options =>
                {
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                    var pooledConn = await connectionPoolManager.GetDbConnectionAsync(DataConsts.ConnStrKey,
                        createNonPooledConnIfExceedLimit: false);

                    if (pooledConn != null)
                        options.UseSqlServer(pooledConn);
                    else options.UseSqlServer(connStr);
                });
            }
            else
            {
                services.AddDbContext<DataContext>(options => options
                    .UseSqlServer(connStr)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            }

            return services;
        }

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
