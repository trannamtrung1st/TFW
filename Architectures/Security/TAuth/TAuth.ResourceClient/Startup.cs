using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceClient.Auth.Policies;
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
            services.AddHttpContextAccessor();

            services.AddHttpClient<IResourceService, ResourceService>(opt =>
            {
                opt.BaseAddress = new Uri(AppSettings.ResourceApiUrl);
            }).AddUserAccessTokenHandler();

            services.AddHttpClient<IIdentityService, IdentityService>(opt =>
            {
                opt.BaseAddress = new Uri(AppSettings.IdpUrl);
            }).AddUserAccessTokenHandler();

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
                opt.Events.OnSigningOut = async e =>
                {
                    // revoke refresh token on sign-out
                    await e.HttpContext.RevokeUserRefreshTokenAsync();
                };
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
            {
                opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.Authority = "https://localhost:5001/";
                opt.ClientId = "resource-client-id";
                opt.ResponseType = OpenIdConnectResponseType.Code;
                //opt.UsePkce = false;
                opt.Scope.Add("email");
                opt.Scope.Add("address");
                opt.Scope.Add("roles");
                opt.Scope.Add("offline_access");
                opt.Scope.Add("resource_api.full");
                opt.SaveTokens = true; // HttpContext.GetTokenAsync
                opt.ClientSecret = "resource-client-secret";
                opt.GetClaimsFromUserInfoEndpoint = true;
                opt.ClaimActions.DeleteClaim(JwtRegisteredClaimNames.Sid);
                opt.ClaimActions.DeleteClaim(JwtRegisteredClaimNames.AuthTime);
                opt.ClaimActions.DeleteClaim("s_hash");
                opt.ClaimActions.DeleteClaim("idp");
                opt.ClaimActions.MapUniqueJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role);
                opt.ClaimActions.MapUniqueJsonKey(JwtClaimTypes.Address, JwtClaimTypes.Address);
                opt.ClaimActions.MapUniqueJsonKey(JwtClaimTypes.EmailVerified, JwtClaimTypes.EmailVerified);

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });

            // adds user and client access token management
            services.AddAccessTokenManagement(options =>
            {
                // client config is inferred from OpenID Connect settings
                // if you want to specify scopes explicitly, do it here, otherwise the scope parameter will not be sent
                //options.Client.Scope = "resource_api.full";
            }).ConfigureBackchannelHttpClient()
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            }));

            // registers HTTP client that uses the managed client access token
            services.AddClientAccessTokenClient("use_client_credentials_to_get_access_token", configureClient: client =>
            {
                client.BaseAddress = new Uri(AppSettings.ResourceApiUrl);
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("CanCreateResource", builder => builder.Requirements.Add(
                    new CreateResourceRequirement()
                    {
                        AllowedCountries = new[] { "Vietnam", "Germany" }
                    }));

                opt.AddPolicy("IsAdmin", builder => builder.RequireRole("Administrator"));

                opt.AddPolicy("EmailVerified", builder => builder.RequireClaim(JwtClaimTypes.EmailVerified, $"{true}"));
            });

            var allAuthHandlers = typeof(Startup).Assembly.GetTypes()
                .Where(type => typeof(IAuthorizationHandler).IsAssignableFrom(type)).ToArray();

            foreach (var handler in allAuthHandlers)
                services.AddSingleton(typeof(IAuthorizationHandler), handler);

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
