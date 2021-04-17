using Microsoft.AspNetCore.Builder;
using Serilog;

namespace TFW.Framework.Logging.Serilog.Web
{
    public static class IApplicationBuilderExtensions
    {
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
                if (frameworkOptions.GetLevel != null)
                    options.GetLevel = (httpContext, elapsed, ex) => frameworkOptions.GetLevel.Value;

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
