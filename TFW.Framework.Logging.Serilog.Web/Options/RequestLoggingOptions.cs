using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Logging.Serilog.Web.Options
{
    public class RequestLoggingOptions
    {
        public string MessageTemplate { get; set; } =
            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        public LogEventLevel GetLevel { get; set; } = LogEventLevel.Information;
        public IDictionary<string, string> EnrichHeaders { get; set; } = new Dictionary<string, string>();
        public bool IncludeHost { get; set; } = false;
    }
}
