using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System;

namespace TFW.Framework.Logging.Serilog.Web
{
    public static class IConfigurationExtensions
    {
        public static Logger ParseLogger(this IConfiguration configuration,
            string sectionName, IServiceProvider provider = null)
        {
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration, sectionName);

            if (provider != null)
                loggerConfig = loggerConfig.ReadFrom.Services(provider);

            return loggerConfig.CreateLogger();
        }
    }
}
