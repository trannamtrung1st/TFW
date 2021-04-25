using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Cross.Requirements;
using TFW.Framework.Common.Helpers;
using TFW.Framework.Configuration;
using TFW.Framework.Data;
using TFW.Framework.Data.Options;
using TFW.Framework.Data.SqlServer;
using TFW.Framework.EFCore;
using TFW.Framework.i18n;
using TFW.Framework.Localization.Json;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Web;
using TFW.Framework.Web.Bindings;
using TFW.Framework.Web.Options;
using TFW.Docs.WebApi.Filters;
using TFW.Docs.WebApi.Handlers;
using TFW.Docs.WebApi.Middlewares;
using TFW.Docs.WebApi.Providers;
using TFW.Docs.WebApi.Controllers;
using TFW.Docs.Data;

namespace TFW.Docs.WebApi
{
    internal static class StartupConfig
    {
        public static IEnumerable<Assembly> TempAssemblyList { get; private set; }
        public static FrameworkOptionsBuilder FrameworkOptionsBuilder { get; private set; }

        public static void Setup()
        {
            TempAssemblyList = ReflectionHelper.GetAllAssemblies(
                excludedRelativeDirPaths: WebApiConsts.ExcludedAssemblyDirs);

            FrameworkOptionsBuilder = new FrameworkOptionsBuilder();
            FrameworkOptionsBuilder.ScanShouldSkipFilterTypes(
                typeof(Startup).Assembly, new[] { typeof(BaseApiController).Namespace });
        }

        public static void Clean()
        {
            TempAssemblyList = null;
            FrameworkOptionsBuilder = null;
        }

        public static IServiceCollection AddHttpBusinessContextProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IBusinessContextProvider, HttpBusinessContextProvider>();
        }

        public static IServiceCollection AddAppDbContext(this IServiceCollection services, ISecretsManager secretsManager)
        {
            string connStr = secretsManager.Get(DataConsts.ConnStrKey);

            if (connStr is null) throw new ArgumentNullException(nameof(connStr));

            services.AddNullConnectionPoolManager()
                .AddDbContext<DataContext>(options => options
                    .UseSqlServer(connStr)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            return services;
        }

        public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            }).AddRoles<AppRole>()
                .AddDefaultTokenProviders()
                .AddSignInManager()
                .AddEntityFrameworkStores<DataContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

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

        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(Policy.Name.AdminOrOwner, policy => policy.RequireAuthenticatedUser()
                    .AddRequirements(new AdminOrOwnerRequirement()));

                opt.AddLogicGroup();
            });

            services.AddSingleton<IAuthorizationHandler, AdminOrOwnerAuthorizationHandler>();

            services.AddAuthUserAuthorizationHandler();

            services.AddLogicGroupAuthorizationHandler();

            services.AddDynamicAuthorizationPolicyProvider(opt =>
            {
                opt.Seperator = DynamicAuthorizationPolicyProviderOptions.DefaultSeperator;
                opt.DefaultSchemes = new[] { JwtBearerDefaults.AuthenticationScheme };

                opt.Providers[Policy.Name.AdminOrOwner] = (paramList, builder) =>
                    builder.AddRequirements(new AdminOrOwnerRequirement(paramList[0]));

                opt.ConfigureAuthUserDynamicPolicy(Policy.Name.AuthUser);
            });

            return services;
        }

        public static IServiceCollection AddWebFrameworks(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = ConfigConsts.i18n.ResourcesPath);

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new QueryObjectModelBinderProvider());

                options.Filters.Add<AutoValidateActionFilter>();

            }).AddNewtonsoftJson()
                .AddDefaultFluentValidation(new[] { typeof(Cross.AssemblyModel).Assembly })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            return services;
        }

        public static IServiceCollection AddAppSwagger(this IServiceCollection services)
        {
            var appSettings = Settings.Get<AppSettings>();

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                c.OperationFilter<SwaggerSecurityOperationFilter>();

                if (appSettings.Swagger.AddSwaggerAcceptLanguageHeader)
                    c.OperationFilter<SwaggerGlobalHeaderOperationFilter>();

                if (appSettings.Swagger.AddSwaggerTimeZoneHeader)
                    c.OperationFilter<SwaggerTimeZoneHeaderOperationFilter>();

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });

                var requirement = new OpenApiSecurityRequirement();
                requirement[new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                }] = Array.Empty<string>();
                c.AddSecurityRequirement(requirement);

                var filePath = Path.Combine(System.AppContext.BaseDirectory,
                    $"{typeof(Startup).Assembly.GetName().Name}.xml");
                c.IncludeXmlComments(filePath);
            });

            return services;
        }

        public static IServiceCollection ConfigureAppOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services.ConfigureSettings(configuration)
                .ConfigureInternationalization()
                .ConfigureApi()
                .ConfigureGlobalQueryFilter(new[] { typeof(DataContext).Assembly })
                .ConfigureFrameworkOptions(FrameworkOptionsBuilder);
        }

        public static IServiceCollection ConfigureApi(this IServiceCollection services)
        {
            return services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
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
                })
                .ConfigureAppRequestTimeZone();
        }

        public static IServiceCollection ConfigureAppRequestTimeZone(this IServiceCollection services)
        {
            return services.Configure<HeaderClientTimeZoneProviderOptions>(opt =>
            {
                opt.HeaderName = HeaderClientTimeZoneProviderOptions.DefaultHeaderName;
            }).Configure<HeaderTimeZoneProviderOptions>(opt =>
            {
                opt.HeaderName = HeaderTimeZoneProviderOptions.DefaultHeaderName;
            }).ConfigureRequestTimeZoneDefault(opt =>
            {
                // First detection will be used
                opt.AddHeader().AddHeaderClient();
            });
        }

        public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)))
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        }

        public static IApplicationBuilder UseRequestDataExtraction(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestDataExtractionMiddleware>();
        }
    }
}
