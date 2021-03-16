using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Configuration.Helpers
{
    public static class ConfigurationHelper
    {
        public static T Parse<T>(this IConfiguration configuration, string key = null)
        {
            if (key != null)
                return configuration.GetSection(key).Get<T>();

            return configuration.Get<T>();
        }

        public static bool TryParse<T>(this IConfiguration configuration, out T output,
            string key = null, Predicate<T> predicate = null)
        {
            if (key != null)
                output = configuration.GetSection(key).Get<T>();
            else output = configuration.Get<T>();

            return predicate?.Invoke(output) ?? true;
        }
    }
}
