using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TFW.Framework.Configuration.Options;

namespace TFW.Framework.Configuration
{
    public interface IJsonConfigurationManager
    {
        T ParseCurrent<T>();
        IDictionary<string, object> ParseCurrent();
        void SaveConfig(object config, Formatting formatting = Formatting.Indented);
    }

    public class JsonConfigurationManager : IJsonConfigurationManager
    {
        private readonly FileInfo _configFileInfo;

        public JsonConfigurationManager(IOptions<JsonConfigurationManagerOptions> options)
        {
            var optionsVal = options.Value;

            if (string.IsNullOrWhiteSpace(optionsVal.ConfigFilePath))
                throw new ArgumentException(nameof(optionsVal.ConfigFilePath));

            if (File.Exists(optionsVal.ConfigFilePath))
                _configFileInfo = new FileInfo(optionsVal.ConfigFilePath);
            else if (!string.IsNullOrWhiteSpace(optionsVal.FallbackFilePath) && File.Exists(optionsVal.FallbackFilePath))
                _configFileInfo = new FileInfo(optionsVal.FallbackFilePath);
            else throw new FileNotFoundException();
        }

        public T ParseCurrent<T>()
        {
            var fileContent = File.ReadAllText(_configFileInfo.FullName);

            var obj = JsonConvert.DeserializeObject<T>(fileContent);

            return obj;
        }

        public IDictionary<string, object> ParseCurrent()
        {
            return ParseCurrent<IDictionary<string, object>>();
        }

        public void SaveConfig(object config, Formatting formatting = Formatting.Indented)
        {
            var newConfigText = JsonConvert.SerializeObject(config, formatting);

            File.WriteAllText(_configFileInfo.FullName, newConfigText);
        }
    }
}
