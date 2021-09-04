using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Configuration.Extensions;

namespace TFW.Framework.Configuration
{
    public interface ISecretsManager
    {
        T Get<T>(string key = null, string prodKey = null,
           IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
        string Get(string key = null, string prodKey = null,
            IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
        Task SetAsync(string key, string value, string prodKey = null, string project = null, string workingDir = "",
            EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
        Task RemoveAsync(string key, string prodKey = null, string project = null, string workingDir = "",
            EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
    }

    public class SecretsManager : ISecretsManager
    {
        public string CmdLineProgram { get; set; }
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

        public SecretsManager() { }

        public T Get<T>(string key = null, string prodKey = null,
            IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
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
            IConfiguration configuration = null, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
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

        public Task SetAsync(string key, string value, string prodKey = null, string project = null, string workingDir = "",
            EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            if (CmdLineProgram == null) throw new InvalidOperationException($"{nameof(CmdLineProgram)} has not been set");

            if (!Env.IsProduction())
            {
                var projectSetting = project != null ? $"--project {project}" : "";

                var process = new Process().Build(
                    CmdLineProgram, arguments: $"/C dotnet user-secrets set \"{key}\" \"{value}\" {projectSetting}",
                    workingDir: workingDir);

                return Task.Run(() =>
                {
                    process.Start();
                    process.WaitForExit();
                });
            }
            else
            {
                Environment.SetEnvironmentVariable(prodKey ?? key, value, target);
                return Task.CompletedTask;
            }
        }

        public Task RemoveAsync(string key, string prodKey = null, string project = null, string workingDir = "",
            EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            if (CmdLineProgram == null) throw new InvalidOperationException($"{nameof(CmdLineProgram)} has not been set");

            if (!Env.IsProduction())
            {
                var projectSetting = project != null ? $"--project {project}" : "";

                var process = new Process().Build(
                    CmdLineProgram, arguments: $"/C dotnet user-secrets remove \"{key}\" {projectSetting}",
                    workingDir: workingDir);

                return Task.Run(() =>
                {
                    process.Start();
                    process.WaitForExit();
                });
            }
            else
            {
                Environment.SetEnvironmentVariable(prodKey ?? key, null, target);
                return Task.CompletedTask;
            }
        }
    }
}
