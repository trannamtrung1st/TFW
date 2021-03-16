using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Logging.Serilog.Web.Options;

namespace TFW.Framework.Logging.Serilog.Web
{
    public static class ConfigHelper
    {
        public static ILogger ParseLogger(this IConfiguration configuration, IServiceProvider provider = null)
        {
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);

            if (provider != null)
                loggerConfig = loggerConfig.ReadFrom.Services(provider);

            return loggerConfig.CreateLogger();
        }

        public static IApplicationBuilder UseDefaultSerilogRequestLogging(this IApplicationBuilder app,
            RequestLoggingOptions frameworkOptions, ILogger logger = null)
        {
            return app.UseSerilogRequestLogging(options =>
            {
                if (!frameworkOptions.UseDefaultLogger)
                    options.Logger = logger;

                // Customize the message template
                options.MessageTemplate = frameworkOptions.MessageTemplate;

                // Emit info-level events instead of the defaults
                options.GetLevel = (httpContext, elapsed, ex) => frameworkOptions.GetLevel;

                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    if (frameworkOptions.IncludeHost)
                        diagnosticContext.Set(nameof(httpContext.Request.Host), httpContext.Request.Host);

                    foreach (var header in frameworkOptions.EnrichHeaders)
                        diagnosticContext.Set(header.Key, httpContext.Request.Headers[header.Value]);
                };
            });
        }
    }
}
