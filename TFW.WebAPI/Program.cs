using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using TFW.Cross;
using TFW.Framework.EFCore.Migration;

namespace TFW.WebAPI
{
    public class Program
    {
        public static int Main(string[] args)
        {
            using var hostLogger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithUtcTimestamp()
                .WriteTo.HostLevelLog()
                .CreateLogger();

            try
            {
                hostLogger.Information("Starting web host");

                var host = CreateHostBuilder(args).Build();

                PrepareApplication(host);

                host.Run();

                hostLogger.Information("Shutdown web host");

                return 0;
            }
            catch (Exception ex)
            {
                hostLogger.Fatal(ex, "Host terminated unexpectedly");

                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Automatically call AddUserSecrets
                //.ConfigureAppConfiguration((hostContext, builder) =>
                //{
                //    if (hostContext.HostingEnvironment.IsDevelopment())
                //    {
                //        builder.AddUserSecrets<Program>();
                //    }
                //})
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration, sectionName: nameof(Serilog))
                    .ReadFrom.Services(services))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                // Automatically perform Validation on Developement
                //.UseDefaultServiceProvider((context, options) =>
                //{
                //    // disable on PROD to boost performance
                //    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                //    options.ValidateOnBuild = true;
                //});

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

    static class ProgramExtensions
    {
        public static LoggerConfiguration HostLevelLog(this LoggerSinkConfiguration writeTo)
        {
            var template = ConfigConsts.Logging.HostLevelLogTemplate;
#if DEBUG
            return writeTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
#else
            return writeTo.File($"{ConfigConsts.Logging.HostLevelLogFolder}/{ConfigConsts.Logging.HostLevelLogFile}",
                rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
#endif
        }
    }
}
