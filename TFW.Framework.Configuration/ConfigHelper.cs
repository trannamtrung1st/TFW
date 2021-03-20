using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Configuration.Options;
using TFW.Framework.Configuration.Services;

namespace TFW.Framework.Configuration
{
    public static class ConfigHelper
    {
        public static IServiceCollection AddDefaultSecretsManager(this IServiceCollection services,
            IHostEnvironment env, IConfiguration configuration, out ISecretsManager secretsManager)
        {
            secretsManager = new SecretsManager()
            {
                DefaultConfiguration = configuration,
                Env = env
            };

            return services.AddSingleton(secretsManager);
        }

        public static IServiceCollection AddSecrectsManager(this IServiceCollection services, ISecretsManager secretsManager)
        {
            return services.AddSingleton(secretsManager);
        }

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
