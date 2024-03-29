﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TFW.Framework.Configuration.Options;

namespace TFW.Framework.Configuration
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultSecretsManager(this IServiceCollection services,
            IHostEnvironment env, IConfiguration configuration,
            string cmdLineProgram, out ISecretsManager secretsManager)
        {
            secretsManager = new SecretsManager()
            {
                DefaultConfiguration = configuration,
                Env = env,
                CmdLineProgram = cmdLineProgram
            };

            return services.AddSingleton(secretsManager);
        }

        public static IServiceCollection AddSecrectsManager(this IServiceCollection services, ISecretsManager secretsManager)
        {
            return services.AddSingleton(secretsManager);
        }

        public static IServiceCollection AddJsonConfigurationManager(this IServiceCollection services,
            string jsonFile = ConfigFiles.AppSettings.Default,
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
