using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Logging.Serilog.Web.Options;

namespace TFW.Framework.Logging.Serilog.Web
{
    public static class ConfigHelper
    {
        public static IApplicationBuilder UseDefaultSerilogRequestLogging(this IApplicationBuilder app,
            RequestLoggingOptions frameworkOptions)
        {
            return app.UseSerilogRequestLogging(options =>
            {
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
