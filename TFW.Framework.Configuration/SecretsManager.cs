using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Configuration.Helpers;

namespace TFW.Framework.Configuration
{
    public interface ISecretsManager
    {
        T Get<T>(string key = null, string prodKey = null,
           IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine);
        string Get(string key = null, string prodKey = null,
            IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine);
    }

    public class SecretsManager : ISecretsManager
    {
        public IConfiguration DefaultConfiguration { get; set; }

        private IHostEnvironment _env;
        public IHostEnvironment Env
        {
            get => _env is null ? throw new InvalidOperationException("Not yet initialized") : _env;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(Env));

                _env = value;
            }
        }

        public T Get<T>(string key = null, string prodKey = null,
            IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine)
        {
            if (!Env.IsProduction())
            {
                return (configuration ?? DefaultConfiguration).Parse<T>(key);
            }
            else
            {
                var str = Environment.GetEnvironmentVariable(prodKey ?? key, target);

                if (str is null) return default;

                return JsonConvert.DeserializeObject<T>(str);
            }
        }

        public string Get(string key = null, string prodKey = null,
            IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Machine)
        {
            if (!Env.IsProduction())
            {
                return (configuration ?? DefaultConfiguration).GetSection(key).Value;
            }
            else
            {
                return Environment.GetEnvironmentVariable(prodKey ?? key, target);
            }
        }
    }
}
