using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using TAuth.Resource.Cross;
using TAuth.ResourceAPI.Auth.Policies;
using TAuth.ResourceAPI.Entities;
using TAuth.ResourceAPI.Services;

namespace TAuth.ResourceAPI
{
    public class Startup
    {
        public static AppSettings AppSettings { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings = new AppSettings();
            Configuration.Bind(nameof(AppSettings), AppSettings);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<ResourceContext>(opt =>
                opt.UseSqlite(Configuration.GetConnectionString(nameof(ResourceContext))));

            services.AddScoped<IUserProvider, UserProvider>();

            // Use classic JWT Bearer
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        opt.Authority = AppSettings.IdpUrl;
            //        opt.Audience = "resource_api";
            //        opt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            NameClaimType = JwtClaimTypes.Name,
            //            RoleClaimType = JwtClaimTypes.Role
            //        };
            //    });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                // JWT tokens
                .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, opt =>
                {
                    opt.Authority = AppSettings.IdpUrl;
                    opt.Audience = "resource_api";
                    //opt.TokenValidationParameters.ClockSkew = TimeSpan.Zero; // demo only: since local will not have clock skew
                })
                // Reference tokens
                .AddOAuth2Introspection(OpenIdConnectConstants.AuthSchemes.Introspection, opt =>
                {
                    opt.Authority = AppSettings.IdpUrl;
                    opt.ClientId = "resource_api";
                    opt.ClientSecret = "resource-api-secret";
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(PolicyNames.Resource.CanDeleteResource, builder => builder
                    .AddRequirements(new DeleteResourceRequirement()
                    {
                        TestName = "Test"
                    }));

                opt.AddPolicy(PolicyNames.IsOwner, builder => builder.AddRequirements(new IsOwnerRequirement()));

                opt.AddPolicy(PolicyNames.IsLucky, builder => builder.RequireAssertion(context => DateTime.UtcNow.Ticks % 10 > 3));

                opt.AddPolicy(PolicyNames.WorkerOnly, builder => builder.RequireAuthenticatedUser().RequireScope("resource_api.background"));

                opt.AddPolicy(PolicyNames.IsAdmin, builder => builder.RequireRole(RoleNames.Administrator));
            });

            var allAuthHandlers = typeof(Startup).Assembly.GetTypes()
                .Where(type => typeof(IAuthorizationHandler).IsAssignableFrom(type)).ToArray();

            foreach (var handler in allAuthHandlers)
                services.AddSingleton(typeof(IAuthorizationHandler), handler);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                #region Bearer/Api Key
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = HeaderNames.Authorization,
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[0]
                    }
                });
                #endregion
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //.RequireAuthorization();
            });
        }
    }
}
