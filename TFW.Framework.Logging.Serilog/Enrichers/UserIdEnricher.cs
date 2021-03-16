using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace TFW.Framework.Logging.Serilog.Enrichers
{
    public class UserIdEnricher : ILogEventEnricher
    {
        public static class Properties
        {
            public const string UserId = nameof(UserId);
        }


        public UserIdEnricher()
        {
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var userId = propertyFactory.CreateProperty(Properties.UserId, ClaimsPrincipal.Current.Identity.Name);

            logEvent.AddPropertyIfAbsent(userId);
        }
    }
}
