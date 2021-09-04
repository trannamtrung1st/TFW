using Microsoft.Extensions.Configuration;
using System;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Setting;
using TFW.Framework.DI.Attributes;
using TFW.Framework.Web.Handlers;

namespace TFW.Docs.WebApi.Handlers
{
    [SingletonService(ServiceType = typeof(IOptionsChangeHandler<JwtSettings>))]
    public class JwtSettingsChangeHandler : OptionsChangeHandler<JwtSettings>
    {
        public JwtSettingsChangeHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override Action<JwtSettings, string> OnChangeAction => (options, name) => Settings.Set(options);
    }
}
