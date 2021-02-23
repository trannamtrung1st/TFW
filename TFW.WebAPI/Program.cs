using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TFW.Framework.EFCore.Migration;

namespace TFW.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            PrepareApplication(host);
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void PrepareApplication(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            
            if (env.IsDevelopment())
            {
                // Auto migration
                var dbContext = serviceProvider.GetRequiredService<DbContext>();
                var dbMigrator = serviceProvider.GetRequiredService<IDbMigrator>();
            
                dbMigrator.CreateOrMigrateDatabase(dbContext);
                
                dbContext.SaveChanges();
            }
        }
    }
}
