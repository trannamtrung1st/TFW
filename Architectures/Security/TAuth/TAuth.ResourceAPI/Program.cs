using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceAPI.Entities;

namespace TAuth.ResourceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var dbContext = provider.GetRequiredService<ResourceContext>();

                var isInit = !dbContext.Database.GetAppliedMigrations().Any();

                dbContext.Database.Migrate();

                if (isInit)
                {
                    dbContext.Resources.AddRange(
                        new ResourceEntity
                        {
                            Name = "Sample Resource 1",
                        },
                        new ResourceEntity
                        {
                            Name = "Sample Resource 2"
                        });

                    dbContext.SaveChanges();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
