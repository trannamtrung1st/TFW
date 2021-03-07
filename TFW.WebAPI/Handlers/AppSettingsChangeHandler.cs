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
    [SingletonService(ServiceType = typeof(IOptionsChangeHandler<AppSettings>))]
    public class AppSettingsChangeHandler : BaseOptionsChangeHandler<AppSettings>
    {
        public AppSettingsChangeHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override Action<AppSettings, string> OnChangeAction => (options, name) => Settings.App = options;
    }
}
