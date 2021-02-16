using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TFW.Business.Extensions;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Extensions;
using TFW.Cross.Models;
using TFW.Data;
using TFW.Data.Extensions;
using TFW.Framework.Common;
using TFW.Framework.DI;
using TFW.Framework.EFCore;
using TFW.Framework.WebAPI.Bindings;
using TFW.Framework.WebAPI.Extensions;
using TFW.Framework.WebConfiguration;
using TFW.WebAPI.Extensions;
using TFW.WebAPI.Middlewares;
using TFW.WebAPI.Models;

namespace TFW.WebAPI
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
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
            var connStr = Configuration.GetConnectionString(Data.DataConsts.ConnStrKey);
            var assemblies = ReflectionHelper.GetAllAssemblies();

            services.AddDbContext<DataContext>(options => options
                    .UseSqlServer(connStr)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking))
                .AddDefaultDbMigrator()
                .AddDefaultTimezoneResolver(TimeZoneConsts.TimezoneMap)
                .AddDefaultDateTimeModelBinder()
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                }).ConfigureCross()
                .ConfigureData()
                .ConfigureBusiness()
                .ScanServices(assemblies);

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
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Settings.Jwt.Issuer,
                        ValidAudience = Settings.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.Default.GetBytes(Settings.Jwt.SecretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
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

            services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new QueryObjectModelBinderProvider());
            }).AddNewtonsoftJson();

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
                }] = new string[] { };
                c.AddSecurityRequirement(requirement);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime,
            IDynamicLinkCustomTypeProvider dynamicLinkCustomTypeProvider)
        {
            // AutoMapper
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetAssembly(typeof(GlobalResources)));
            });
            GlobalResources.Mapper = mapConfig.CreateMapper();

            // Dynamic Linq
            GlobalResources.CustomTypeProvider = dynamicLinkCustomTypeProvider;

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

            app.UsePrincipalInfoMiddleware();

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
            // handle application started
        }
    }
}
