using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            #region Quartz basic
            StartQuartzExampleAsync().Wait();
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

        public class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= Quartz.Logging.LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }
        }

        public static async Task StartQuartzExampleAsync()
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .UsingJobData(nameof(HelloJob.MyParam), "Hello my param")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);

            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(10));

            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();
        }
    }

    public class HelloJob : IJob
    {
        public string MyParam { get; set; }

        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync($"Greetings from HelloJob! {MyParam}");
        }
    }
}
