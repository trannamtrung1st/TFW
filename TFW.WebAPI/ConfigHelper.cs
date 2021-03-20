using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using TFW.Cross;
using TFW.Cross.Models.Setting;
using TFW.Cross.Providers;
using TFW.Cross.Requirements;
using TFW.Data.Core;
using TFW.Data.Providers;
using TFW.Framework.Data;
using TFW.Framework.Data.Options;
using TFW.Framework.Data.SqlServer;
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
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment env)
        {
            string connStr;

            if (!env.IsProduction())
            {
                connStr = configuration.GetConnectionString(DataConsts.ConnStrKey);
            }
            else
            {
                connStr = Environment.GetEnvironmentVariable(DataConsts.EnvConnStrKey, EnvironmentVariableTarget.User);
            }

            if (Settings.App.UseDbConnectionPool)
            {
                #region Event handlers
                void Pool_TryReturnToPoolError(Exception ex, int tryCount)
                {
                    Log.Error(ex, "Failed {TryCount} time(s) in retrying add DbConnection to pool", tryCount);
                }

                void Pool_WatcherThreadError(Exception ex, DbConnection dbConnection)
                {
                    Log.Error(ex, "Failure on watcher thread with DbConnection of '{ConnStr}'", dbConnection.ConnectionString);
                }

                void Pool_NewConnectionError(Exception ex, string poolKey)
                {
                    Log.Error(ex, "Failure on create connection '{PoolKey}'", poolKey);
                }

                void Pool_ReleaseConnectionError(Exception ex, DbConnection dbConnection)
                {
                    Log.Error(ex, "Failure on disposing DbConnection of '{ConnStr}'", dbConnection.ConnectionString);
                }
                #endregion

                IDbConnectionPoolManager connectionPoolManager;

                var poolKeyMap = new Dictionary<string, string>();

                services.AddSqlConnectionPoolManager(out connectionPoolManager, configAction: options =>
                {
                    options.WatchIntervalInMinutes = SqlConnectionPoolManagerOptions.DefaultWatchIntervalInMinutes;
                }, initAction: async pool =>
                {
                    pool.TryReturnToPoolError += Pool_TryReturnToPoolError;
                    pool.NewConnectionError += Pool_NewConnectionError;
                    pool.WatcherThreadError += Pool_WatcherThreadError;
                    pool.ReleaseConnectionError += Pool_ReleaseConnectionError;

                    var poolSize = SqlConnectionHelper.ReadPoolSize(connStr);

                    var poolKey = await pool.InitDbConnectionAsync(new ConnectionPoolOptions
                    {
                        ConnectionString = connStr,
                        LifetimeInMinutes = ConnectionPoolOptions.DefaultLifetimeInMinutes,
                        MaximumRetryWhenFailure = ConnectionPoolOptions.DefaultMaximumRetryWhenFailure,
                        RetryIntervalInSeconds = ConnectionPoolOptions.DefaultRetryIntervalInSeconds,
                        MaxPoolSize = poolSize.maxPoolSize,
                        MinPoolSize = poolSize.minPoolSize
                    });

                    poolKeyMap[DataConsts.ConnStrKey] = poolKey;

                }).AddDbContext<DataContext>(async builder =>
                {
                    var pooledConn = await connectionPoolManager.GetDbConnectionAsync(poolKeyMap[DataConsts.ConnStrKey]);

                    builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                    if (pooledConn != null)
                        builder.UseSqlServer(pooledConn);
                    else builder.UseSqlServer(connStr);
                });
            }
            else
            {
                services.AddNullConnectionPoolManager()
                    .AddDbContext<DataContext>(options => options
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
