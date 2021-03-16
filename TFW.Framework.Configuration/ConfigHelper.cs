using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Configuration.Options;
using TFW.Framework.Configuration.Services;

namespace TFW.Framework.Configuration
{
    public static class ConfigHelper
    {
        public static IServiceCollection AddJsonConfigurationManager(this IServiceCollection services,
            string jsonFile = CommonConsts.AppSettings.Default,
            string fallbackJsonFile = null,
            IJsonConfigurationManager customManager = null)
        {
            if (customManager != null)
                return services.AddSingleton(customManager);

            return services.Configure<JsonConfigurationManagerOptions>(opt =>
            {
                opt.ConfigFilePath = jsonFile;
                opt.FallbackFilePath = fallbackJsonFile;
            }).AddSingleton<IJsonConfigurationManager, JsonConfigurationManager>();
        }
    }
}
