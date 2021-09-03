using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace TFW.Framework.Background
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Hangfire basic
            //GlobalConfiguration.Configuration
            //    .UseSqlServerStorage(@"Server=localhost;Database=TFW.Hangfire;Trusted_Connection=True;MultipleActiveResultSets=true");

            //BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

            //BackgroundJob.Schedule(() => Console.WriteLine("Scheduled job"), DateTimeOffset.UtcNow.AddSeconds(5));

            //RecurringJob.AddOrUpdate(nameof(WriteTime), () => WriteTime(), "*/5 * * * *");

            //using (new BackgroundJobServer())
            //{
            //    CreateHostBuilder(args).Build().Run();
            //}
            #endregion

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void WriteTime()
        {
            Console.WriteLine(DateTimeOffset.UtcNow);
        }
    }
}
