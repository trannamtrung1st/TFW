using Microsoft.Extensions.Configuration;
using System;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Setting;
using TFW.Framework.DI.Attributes;
using TFW.Framework.Web.Handlers;

namespace TFW.Docs.WebApi.Handlers
{
    [SingletonService(ServiceType = typeof(IOptionsChangeHandler<AppSettings>))]
    public class AppSettingsChangeHandler : OptionsChangeHandler<AppSettings>
    {
        public AppSettingsChangeHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override Action<AppSettings, string> OnChangeAction => (options, name) => Settings.Set(options);
    }
}
