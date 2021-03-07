using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Cross.Models.Setting;
using TFW.Framework.DI.Attributes;
using TFW.Framework.Web.Handlers;

namespace TFW.WebAPI.Handlers
{
    [SingletonService(ServiceType = typeof(IOptionsChangeHandler<JwtSettings>))]
    public class JwtSettingsChangeHandler : BaseOptionsChangeHandler<JwtSettings>
    {
        public JwtSettingsChangeHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override Action<JwtSettings, string> OnChangeAction => (options, name) => Settings.Jwt = options;
    }
}
