using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Models.Setting;
using TFW.Cross.Providers;
using TFW.Data;
using TFW.Data.Core;
using TFW.Framework.AutoMapper;
using TFW.Framework.Common.Helpers;
using TFW.Framework.Configuration;
using TFW.Framework.Configuration.Helpers;
using TFW.Framework.DI;
using TFW.Framework.EFCore;
using TFW.Framework.i18n;
using TFW.Framework.SimpleMail;
using TFW.Framework.SimpleMail.Options;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Web;
using TFW.Framework.Web.Bindings;
using TFW.Framework.Web.Options;
using TFW.WebAPI.Controllers;
using TFW.WebAPI.Filters;
using TFW.WebAPI.Models;

namespace TFW.WebAPI
{
    public class Startup
    {
        private static IEnumerable<Assembly> _tempAssemblyList;
        private const string _defaultJsonFile = TFW.Framework.Configuration.CommonConsts.DefaultAppSettingsFile;
        private readonly string _envJsonFile;

        public Startup(IWebHostEnvironment env)
        {
            _envJsonFile = $"appsettings.{env.EnvironmentName}.json";

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(_defaultJsonFile, optional: true, reloadOnChange: true)
                .AddJsonFile(_envJsonFile, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Settings.App = Configuration.Parse<AppSettings>(nameof(AppSettings));
            Settings.Jwt = Configuration.Parse<JwtSettings>(nameof(JwtSettings));
            Configuration.Bind(nameof(ApiSettings), ApiSettings.Instance);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Common
            var connStr = Configuration.GetConnectionString(DataConsts.ConnStrKey);

            _tempAssemblyList = ReflectionHelper.GetAllAssemblies(
                excludedRelativeDirPaths: WebApiConsts.ExcludedAssemblyDirs);
            #endregion

            #region Framework options
            var fwOptionsConfigurator = new FrameworkOptionsConfigurator();

            fwOptionsConfigurator.ScanShouldSkipFilterTypes(
                typeof(Startup).Assembly, new[] { typeof(BaseApiController).Namespace });
            #endregion

            #region Services
            services.AddDbContext<DataContext>(options => options
                    .UseSqlServer(connStr)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddHttpContextAccessor()
                .AddHttpBusinessContextProvider()
                .AddHttpUnitOfWorkProvider()
                .ScanServices(_tempAssemblyList)
                .AddDefaultDbMigrator()
                .AddDefaultDateTimeModelBinder()
                .AddRequestTimeZoneMiddleware()
                .AddDefaultValidationResultProvider()
                .AddSmtpService(Configuration.GetSection(nameof(SmtpOption)))
                .AddJsonConfigurationManager(_defaultJsonFile, _envJsonFile)
                .ConfigureAppOptions(Configuration)
                .ConfigureRequestTimeZoneDefault()
                .ConfigureGlobalQueryFilter(new[] { typeof(DataContext).Assembly })
                .ConfigureFrameworkOptions(fwOptionsConfigurator);
            #endregion

            #region OAuth
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
            #endregion

            #region Mvc, Controllers
            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new QueryObjectModelBinderProvider());

                options.Filters.Add<AutoValidateActionFilter>();

            }).AddNewtonsoftJson()
                .AddDefaultFluentValidation(new[] { typeof(Cross.AssemblyModel).Assembly });
            #endregion

            #region Swagger
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
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime,
            IDynamicLinkCustomTypeProvider dynamicLinkCustomTypeProvider)
        {
            // Configurations
            app.RegisterOptionsChangeHandlers(typeof(AppSettings),
                typeof(JwtSettings),
                typeof(ApiSettings));

            // AutoMapper
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(_tempAssemblyList);
            });
            GlobalMapper.Init(mapConfig.CreateMapper());

            // Dynamic Linq
            DynamicLinqEntityTypeProvider.DefaultParsingConfig = new ParsingConfig
            {
                CustomTypeProvider = dynamicLinkCustomTypeProvider
            };

            // i18n
            Time.Providers.Default = Time.Providers.Utc;

            // HttpContext
            app.ConfigureHttpContext();

            // BusinessContext
            app.ConfigureBusinessContext();

            // UnitOfWork
            app.ConfigureUnitOfWork();

            PrepareEnvironment(env);

            if (env.IsDevelopment())
            {
                // configure dev settings
            }

            app.UseExceptionHandler($"/{ApiEndpoint.Error}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                //builder.AllowAnyOrigin();
                builder.SetIsOriginAllowed(origin =>
                {
                    return true;
                });
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseRequestDataExtraction();

            app.UseRequestTimeZone();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // app lifetime
            appLifetime.ApplicationStarted.Register(OnApplicationStarted);
        }

        private void PrepareEnvironment(IWebHostEnvironment env)
        {
            // create directories ...
        }

        private void OnApplicationStarted()
        {
            // clear caching
            _tempAssemblyList = null;
        }
    }
}
