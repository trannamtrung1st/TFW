using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceClient.Auth.Policies;
using TAuth.ResourceClient.Handlers;
using TAuth.ResourceClient.Services;

namespace TAuth.ResourceClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AppSettings = new AppSettings();
            Configuration.Bind(nameof(AppSettings), AppSettings);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public AppSettings AppSettings { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor()
                .AddTransient<BearerTokenHandler>();

            services.AddHttpClient<IResourceService, ResourceService>(opt =>
            {
                opt.BaseAddress = new Uri(AppSettings.ResourceApiUrl);
            }).AddHttpMessageHandler<BearerTokenHandler>();

            services.AddHttpClient<IIdentityService, IdentityService>(opt =>
            {
                opt.BaseAddress = new Uri(AppSettings.IdpUrl);
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                opt.AccessDeniedPath = new PathString("/accessdenied");
                opt.Events.OnRedirectToAccessDenied = (context) =>
                {
                    context.RedirectUri += $"#{DateTimeOffset.UtcNow.Ticks}";
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
            {
                opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.Authority = "https://localhost:5001/";
                opt.ClientId = "resource-client-id";
                opt.ResponseType = "code";
                //opt.UsePkce = false;
                opt.Scope.Add("address");
                opt.Scope.Add("roles");
                opt.Scope.Add("resource_api.full");
                opt.SaveTokens = true;
                opt.ClientSecret = "resource-client-secret";
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.ClaimActions.DeleteClaim(JwtRegisteredClaimNames.Sid);
                opt.ClaimActions.DeleteClaim(JwtRegisteredClaimNames.AuthTime);
                opt.ClaimActions.DeleteClaim("s_hash");
                opt.ClaimActions.DeleteClaim("idp");
                opt.ClaimActions.MapUniqueJsonKey(JwtClaimTypes.Role, "role");

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("CanDeleteResource", builder => builder
                    .AddRequirements(new DeleteResourceRequirement()
                    {
                        TestName = "Test"
                    }));

                opt.AddPolicy("CanCreateResource", builder => builder.AddRequirements(
                    new CreateResourceRequirement()
                    {
                        AllowedCountries = new[] { "Vietnam", "Germany" }
                    }));

                opt.AddPolicy("IsOwner", builder => builder.AddRequirements(new IsOwnerRequirement()));

                opt.AddPolicy("IsAdmin", builder => builder.RequireRole("Administrator"));

                opt.AddPolicy("EmailVerified", builder => builder.RequireClaim(JwtClaimTypes.EmailVerified, $"{true}"));
            });

            var allAuthHandlers = typeof(Startup).Assembly.GetTypes().OfType<IAuthenticationHandler>().ToArray();

            foreach (var handler in allAuthHandlers)
                services.AddSingleton(typeof(IAuthenticationHandler), handler);

            services.AddRazorPages(opt =>
            {
                opt.Conventions.AuthorizeFolder("/");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
