using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Logging;
using System;
using System.Threading.Tasks;
using TFW.Framework.Background.Services;

namespace TFW.Framework.Background
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // base configuration from appsettings.json
            services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));

            // if you are using persistent job store, you might want to alter some options
            services.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true; // default: false
                options.Scheduling.OverWriteExistingData = true; // default: true
            });

            services.AddQuartz();

            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            services.AddScoped<DisposableService>()
                .AddScoped((_) => new ChildDisposableService());

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    // Each background worker will refresh the timeout to let other workers know the job is being processed
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(1),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddAuthorization();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                #region Bearer/Api Key
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = HeaderNames.Authorization,
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[0]
                    }
                });
                #endregion
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            DisposableService disposableService,
            ISchedulerFactory schedulerFactory)
        {
            #region Quartz basic
            StartQuartzExampleAsync(schedulerFactory).Wait();
            #endregion

            //disposableService.Process();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            // Can be replaced by MapHangfireDashboard
            //app.UseHangfireDashboard(pathMatch:
            //    "/internal/hangfire", options: new DashboardOptions
            //    {
            //        //AsyncAuthorization = new[]
            //        //{
            //        //    new AppAuthorizeFilter()
            //        //},
            //        IsReadOnlyFunc = (context) => true,
            //    });

            BackgroundJob.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard("/internal/hangfire", options: new DashboardOptions
                {
                    IsReadOnlyFunc = (context) => true
                });
                //.RequireAuthorization();
            });
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

        public class HelloJob : IJob
        {
            public string MyParam { get; set; }

            public async Task Execute(IJobExecutionContext context)
            {
                await Console.Out.WriteLineAsync($"Greetings from HelloJob! {MyParam}");
            }
        }

        public static async Task StartQuartzExampleAsync(ISchedulerFactory schedulerFactory)
        {
            var scheduler = await schedulerFactory.GetScheduler();
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            // and start it off
            //await scheduler.Start();

            var trigger = await scheduler.GetTrigger(new TriggerKey("trigger1", "group1"));
            if (trigger == null)
            {
                IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .UsingJobData(nameof(HelloJob.MyParam), "Hello my param")
                .Build();

                // Trigger the job to run now, and then repeat every 10 seconds
                trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10))
                    .Build();

                // Tell quartz to schedule the job using our trigger
                await scheduler.ScheduleJob(job, trigger);
            }

            var laterTrigger = await scheduler.GetTrigger(new TriggerKey("Test"));
            if (laterTrigger == null)
            {
                IJobDetail job2 = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job2", "group1")
                    .UsingJobData(nameof(HelloJob.MyParam), "Hello my param ADO STORE")
                    .Build();

                laterTrigger = TriggerBuilder.Create()
                    .WithIdentity("Test")
                    .StartAt(DateTimeOffset.UtcNow.AddSeconds(20))
                    .Build();

                await scheduler.ScheduleJob(job2, laterTrigger);
            }

            // some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromSeconds(10));

            // and last shut down the scheduler when you are ready to close your program
            //await scheduler.Shutdown();
        }

    }
}
