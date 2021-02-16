using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Common
{
    public static class ConfigExtensions
    {
        public static IServiceCollection AddDefaultTimezoneResolver(this IServiceCollection services,
            IDictionary<string, TimeZoneInfo> timeZoneMap)
        {
            return services.AddSingleton<ITimeZoneResolver>(new DefaultTimeZoneResolver(timeZoneMap));
        }
    }
}
