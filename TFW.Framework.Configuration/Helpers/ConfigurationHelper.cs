using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Configuration.Helpers
{
    public static class ConfigurationHelper
    {
        public static T Parse<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Get<T>();
        }

        public static bool TryParse<T>(this IConfiguration configuration, string key,
            out T output, Predicate<T> predicate = null)
        {
            output = configuration.GetSection(key).Get<T>();
            
            return predicate?.Invoke(output) ?? true;
        }
    }
}
