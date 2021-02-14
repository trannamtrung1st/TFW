using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.WebConfiguration
{
    public static class WebConfigurationExtensions
    {
        public static T Parse<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Get<T>();
        }
    }
}
