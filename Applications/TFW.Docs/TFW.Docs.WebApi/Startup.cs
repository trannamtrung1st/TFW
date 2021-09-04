using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Setting;
using TFW.Framework.AutoMapper;
using TFW.Framework.Configuration;
using TFW.Framework.Configuration.Extensions;
using TFW.Framework.DI;
using TFW.Framework.EFCore;
using TFW.Framework.Logging.Serilog.Web;
using TFW.Framework.SimpleMail;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Web;

namespace TFW.Docs.WebApi
{
    public class Startup
    {
        private readonly RequestLoggingOptions _requestLoggingOptions;
        private readonly IWebHostEnvironment _env;
        private readonly List<IDisposable> _resources;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
            _resources = new List<IDisposable>();

            // App Settings
            Settings.Set(Configuration.Parse<AppSettings>(nameof(AppSettings)));
            Settings.Set(Configuration.Parse<JwtSettings>(nameof(JwtSettings)));

            // Serilog
            Settings.Set(Configuration.Parse<SerilogSettings>(nameof(Serilog)));
            _requestLoggingOptions = Configuration.Parse<RequestLoggingOptions>(LoggingConsts.RequestLoggingOptionsKey);

            // Mail
            _smtpOptionSection = Configuration.GetSection(nameof(SmtpOption));
        }

        public IConfiguration Configuration { get; }
        private readonly IConfigurationSection _smtpOptionSection;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            StartupConfig.Setup();

            ISecretsManager secretsManager;
            IServiceInjector serviceInjector;
            IKeyedServiceManager keyedServiceManager;

            services.AddDefaultSecretsManager(_env, Configuration,
                    ConfigConsts.CommandLine.WindowsCmd, out secretsManager)
                .AddKeyedServiceManager(out keyedServiceManager)
                .AddAppDbContext(secretsManager)
                .AddServiceInjector(StartupConfig.TempAssemblyList, out serviceInjector)
                .AddHttpContextAccessor()
                .AddHttpBusinessContextProvider()
                .AddDefaultDbMigrator()
                .AddRequestFeatureMiddleware()
                .AddAppCaching()
                .AddDefaultValidationResultProvider()
                .AddSmtpService(opt =>
                {
                    _smtpOptionSection.Bind(opt);
                    opt.Password = secretsManager.Get(ConfigConsts.Mail.PasswordKey);
                })
                .AddJsonConfigurationManager(Program.DefaultJsonFile, Program.EnvJsonFile)
                .AddAppAuthentication()
                .AddAppAuthorization()
                .AddWebFrameworks()
                .AddAppSwagger()
                .ScanServices(StartupConfig.TempAssemblyList, serviceInjector)
                .ConfigureAppOptions(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime appLifetime,
            IDynamicLinkCustomTypeProvider dynamicLinkCustomTypeProvider,
            ISecretsManager secretsManager)
        {
            // Secrets
            Settings.Get<JwtSettings>().SecretKey = secretsManager.Get(JwtSettings.ConfigKey);

            // Configurations
            app.RegisterOptionsChangeHandlers(typeof(AppSettings),
                typeof(JwtSettings));

            // AutoMapper
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(StartupConfig.TempAssemblyList);
            });
            GlobalMapper.Init(mapConfig.CreateMapper());

            // Dynamic Linq
            DynamicLinqConsts.DefaultParsingConfig = new ParsingConfig
            {
                CustomTypeProvider = dynamicLinkCustomTypeProvider
            };

            // HttpContext
            app.ConfigureHttpContext();

            #region Serilog
            if (!_requestLoggingOptions.UseDefaultLogger)
            {
                var requestLogger = Configuration.ParseLogger(
                    LoggingConsts.RequestLoggingOptionsKey, app.ApplicationServices);
                app.UseDefaultSerilogRequestLogging(_requestLoggingOptions, requestLogger);
                _resources.Add(requestLogger);
            }
            else app.UseDefaultSerilogRequestLogging(_requestLoggingOptions);
            #endregion

            app.UseExceptionHandler($"/{Routing.Controller.Error.Route}");

            app.UseRequestFeature();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRequestLocalization();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
                c.InjectStylesheet("/custom-swagger-ui.css");
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
                //builder.AllowAnyOrigin();
                builder.SetIsOriginAllowed(origin =>
                {
                    return true;
                });
            });

            app.UseAuthentication();

            app.UseRequestDataExtraction();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // app lifetime
            appLifetime.ApplicationStarted.Register(OnApplicationStarted);
            appLifetime.ApplicationStopped.Register(OnApplicationStopped);

            PrepareEnvironment(env);
        }

        private void PrepareEnvironment(IWebHostEnvironment env)
        {
            // create directories ...
        }

        private void OnApplicationStarted()
        {
            StartupConfig.Clean();
        }

        private void OnApplicationStopped()
        {
            Console.WriteLine("Cleaning resources ...");

            foreach (var res in _resources)
                res.Dispose();
        }
    }
}
