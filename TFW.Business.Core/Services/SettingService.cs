using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Business.Services;
using TFW.Cross.Models.Setting;
using TFW.Data.Core;
using TFW.Framework.Configuration.Helpers;
using TFW.Framework.Configuration.Services;
using TFW.Framework.DI.Attributes;
using TFW.Framework.SimpleMail.Options;

namespace TFW.Business.Core.Services
{
    [ScopedService(ServiceType = typeof(ISettingService))]
    public class SettingService : BaseService, ISettingService
    {
        private readonly IJsonConfigurationManager _configurationManager;
        private readonly IConfigurationRoot _configurationRoot;

        public SettingService(DataContext dbContext,
            IJsonConfigurationManager configurationManager,
            IConfiguration configuration) : base(dbContext)
        {
            _configurationManager = configurationManager;
            _configurationRoot = configuration as IConfigurationRoot;
        }

        public void ChangeSmtpOption(ChangeSmtpOptionModel model)
        {
            var config = _configurationManager.ParseCurrent();

            var smtpOption = _configurationRoot.Parse<SmtpOption>(nameof(SmtpOption));

            smtpOption.UserName = model.UserName;
            smtpOption.Password = model.Password;

            config[nameof(SmtpOption)] = smtpOption;

            _configurationManager.SaveConfig(config);
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
