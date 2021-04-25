using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.Cross.Providers;
using TFW.Docs.Data;
using TFW.Framework.Configuration;
using TFW.Framework.Configuration.Extensions;
using TFW.Framework.DI.Attributes;
using TFW.Framework.SimpleMail;

namespace TFW.Docs.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(ISettingService))]
    public class SettingService : BaseService, ISettingService
    {
        private readonly IJsonConfigurationManager _configurationManager;
        private readonly ISecretsManager _secretsManager;
        private readonly IConfigurationRoot _configurationRoot;

        public SettingService(DataContext dbContext,
            IStringLocalizer<ResultCodeResources> resultLocalizer,
            IBusinessContextProvider contextProvider,
            IJsonConfigurationManager configurationManager,
            ISecretsManager secretsManager,
            IConfiguration configuration) : base(dbContext, resultLocalizer, contextProvider)
        {
            _configurationManager = configurationManager;
            _secretsManager = secretsManager;
            _configurationRoot = configuration as IConfigurationRoot;
        }

        public Task<bool> GetInitStatusAsync()
        {
            return dbContext.Users.AnyAsync();
        }

        public async Task ChangeSmtpOptionAsync(ChangeSmtpOptionModel model)
        {
            var config = _configurationManager.ParseCurrent();
            var smtpOption = _configurationRoot.Parse<SmtpOption>(nameof(SmtpOption));

            smtpOption.UserName = model.UserName;
            config[nameof(SmtpOption)] = JToken.FromObject(smtpOption);

            _configurationManager.SaveConfig(config);

            await _secretsManager.SetAsync(ConfigConsts.Mail.PasswordKey, model.Password);
        }

        public void ReloadConfiguration()
        {
            _configurationRoot.Reload();

            var currentConfig = _configurationManager.ParseCurrent();

            // trigger reload for IOptionsMonitor(s)
            _configurationManager.SaveConfig(currentConfig);
        }
    }
}
