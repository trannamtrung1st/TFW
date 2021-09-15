// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.AspNetIdentity;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TAuth.IDP.Models;
using TAuth.IDP.Services;
using TAuth.Resource.Cross.Services;
using TwoStepsAuthenticator;

namespace TAuth.IDP
{
    public class Startup
    {
        public static TimeAuthenticator Authenticator { get; } = new TimeAuthenticator(intervalSeconds: AuthConstants.Mfa.OTPIntervalSeconds);

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //var rsaKey = GetKeyFromContainer("TAuth.IDP");
            //var rsa = RSA.Create();
            //rsa.FromXmlString(rsaKey);

            var signingCert = LoadCertificateFromStore();
            signingCert = LoadCertificateFromFile();

            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            services.AddSingleton<IEmailService, MockEmailService>()
                .AddScoped<IIdentityService, IdentityService>();

            services.AddIdentityCore<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                //options.Tokens.EmailConfirmationTokenProvider = "CustomEmailTokenProvider"; // Change lifetime of email confirmation token
            }).AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
                .AddSignInManager()
                .AddEntityFrameworkStores<IdpContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

                // set to False to prevent single sign-out
                //options.Endpoints.EnableEndSessionEndpoint = false;
            })
                //.AddSigningCredential(new RsaSecurityKey(rsa), IdentityServerConstants.RsaSigningAlgorithm.RS256)
                .AddSigningCredential(signingCert)
                //.AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiResources(Config.ApiResources)
                //.AddInMemoryApiScopes(Config.ApiScopes)
                //.AddInMemoryClients(Config.Clients)
                //.AddTestUsers(TestUsers.Users);
                .AddProfileService<ProfileService<AppUser>>();

            // not recommended for production - you need to store your key material somewhere secure
            //builder.AddDeveloperSigningCredential();

            builder.AddConfigurationStore<IdpContext>(opt =>
            {
                opt.ConfigureDbContext = builder =>
                    builder.UseSqlite(Configuration.GetConnectionString(nameof(IdpContext)));
            });

            builder.AddOperationalStore<IdpContext>(opt =>
            {
                opt.ConfigureDbContext = builder =>
                    builder.UseSqlite(Configuration.GetConnectionString(nameof(IdpContext)));

                // this enables automatic token cleanup. this is optional.
                opt.EnableTokenCleanup = true;
                opt.TokenCleanupInterval = 30;
            });

            services.Configure<IISOptions>(opt =>
            {
                opt.AuthenticationDisplayName = AuthConstants.IIS.AuthDisplayName;
                opt.AutomaticAuthentication = false;
            });

            services.Configure<IISServerOptions>(opt =>
            {
                opt.AuthenticationDisplayName = AuthConstants.IIS.AuthDisplayName;
                opt.AutomaticAuthentication = false;
            });

            services.AddAuthentication(opt =>
            {
                //opt.RequireAuthenticatedSignIn = false;
            })
                .AddCookie(AuthConstants.AuthSchemes.IdentityMfa, opt =>
                {
                    opt.ExpireTimeSpan = AuthConstants.Mfa.DefaultExpireTime;
                    opt.SlidingExpiration = false;
                })
                .AddFacebook(AuthConstants.AuthSchemes.Facebook, "Facebook Login", opt =>
                {
                    // Use UsersSecret or Environment variables to configure
                    opt.AppId = Configuration.GetValue<string>("Facebook:AppId");
                    opt.AppSecret = Configuration.GetValue<string>("Facebook:AppSecret");
                    opt.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;
                })
                .AddGoogle(AuthConstants.AuthSchemes.Google, "Google Login", opt =>
                {
                    // Use UsersSecret or Environment variables to configure
                    opt.ClientId = Configuration.GetValue<string>("Google:ClientId");
                    opt.ClientSecret = Configuration.GetValue<string>("Google:ClientSecret");
                    opt.SignInScheme = IdentityServer4.IdentityServerConstants.ExternalCookieAuthenticationScheme;
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        static string GetKeyFromContainer(string containerName)
        {
            // Create the CspParameters object and set the key container
            // name used to store the RSA key pair.
            var parameters = new CspParameters
            {
                KeyContainerName = containerName
            };

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            using var rsa = new RSACryptoServiceProvider(2048, parameters);

            var key = rsa.ToXmlString(true);
            // Display the key information to the console.
            Console.WriteLine($"Key retrieved from container : \n {key}");

            return key;
        }

        static X509Certificate2 LoadCertificateFromStore()
        {
            var thumbprint = "73d47632f938020fc3009efa5bf23ff54e32b6ae";

            using var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);

            if (certs.Count == 0)
            {
                throw new Exception("Not found certificate");
            }

            return certs[0];
        }

        // Demo only
        static X509Certificate2 LoadCertificateFromFile()
        {
            X509Certificate2 x509 = new X509Certificate2("signing-cert.pfx", "TAuth.IDP", X509KeyStorageFlags.MachineKeySet);
            return x509;
        }
    }
}
