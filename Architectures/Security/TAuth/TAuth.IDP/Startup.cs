// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace TAuth.IDP
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
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

            var builder = services.AddIdentityServer(options =>
            {
                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

                // set to False to prevent single sign-out
                //options.Endpoints.EnableEndSessionEndpoint = false;
            })
                //.AddSigningCredential(new RsaSecurityKey(rsa), IdentityServerConstants.RsaSigningAlgorithm.RS256)
                .AddSigningCredential(signingCert)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(TestUsers.Users);

            // not recommended for production - you need to store your key material somewhere secure
            //builder.AddDeveloperSigningCredential();
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
