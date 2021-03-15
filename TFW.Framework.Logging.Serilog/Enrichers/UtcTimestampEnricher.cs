using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Logging.Serilog.Enrichers
{
    public class UtcTimestampEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
            logEvent.AddPropertyIfAbsent(pf.CreateProperty("UtcTimestamp", logEvent.Timestamp.UtcDateTime));
        }
    }
}
