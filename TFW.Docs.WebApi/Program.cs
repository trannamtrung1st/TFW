using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using TFW.Docs.Cross;
using TFW.Framework.Configuration;
using TFW.Framework.EFCore.Migration;
using TFW.Docs.Data;

namespace TFW.Docs.WebApi
{
    public class Program
    {
        public const string DefaultJsonFile = ConfigFiles.AppSettings.Default;
        public static string EnvJsonFile { get; private set; } = ConfigFiles.AppSettings.DefaultEnv;

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
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    var env = hostContext.HostingEnvironment;
                    EnvJsonFile = EnvJsonFile.Replace(ConfigFiles.AppSettings.EnvPlaceholder, env.EnvironmentName);
                })
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration, sectionName: nameof(Serilog))
                    .ReadFrom.Services(services))
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
                var dbContext = serviceProvider.GetRequiredService<DataContext>();
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
            var template = LoggingConsts.HostLevelLogTemplate;

            if (Environment.GetEnvironmentVariable(EnvironmentVariables.AspNetCoreEnv.Key)
                == EnvironmentVariables.AspNetCoreEnv.Development)
            {
                return writeTo.Console(restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
            }

            return writeTo.File($"{LoggingConsts.HostLevelLogFolder}/{LoggingConsts.HostLevelLogFile}",
                rollingInterval: RollingInterval.Month,
                restrictedToMinimumLevel: LogEventLevel.Information, outputTemplate: template);
        }
    }
}
