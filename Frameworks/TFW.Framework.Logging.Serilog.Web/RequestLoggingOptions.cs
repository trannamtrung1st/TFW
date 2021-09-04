using Serilog.Events;
using System.Collections.Generic;

namespace TFW.Framework.Logging.Serilog.Web
{
    public class RequestLoggingOptions
    {
        public string MessageTemplate { get; set; } =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        public LogEventLevel? GetLevel { get; set; }
        public IDictionary<string, string> EnrichHeaders { get; set; } = new Dictionary<string, string>();
        public bool IncludeHost { get; set; } = false;
        public bool UseDefaultLogger { get; set; } = true;
    }
}
