﻿using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Logging.Serilog.Enrichers;

namespace Serilog
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithUtcTimestamp(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<UtcTimestampEnricher>();
        }
    }
}