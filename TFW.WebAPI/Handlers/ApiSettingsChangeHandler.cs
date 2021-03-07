using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.AutoMapper.Helpers;
using TFW.Framework.DI.Attributes;
using TFW.Framework.Web.Handlers;
using TFW.WebAPI.Models;

namespace TFW.WebAPI.Handlers
{
    [SingletonService(ServiceType = typeof(IOptionsChangeHandler<ApiSettings>))]
    public class ApiSettingsChangeHandler : OptionsChangeHandler<ApiSettings>
    {
        public ApiSettingsChangeHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public override Action<ApiSettings, string> OnChangeAction =>
            (options, name) => ApiSettings.Instance.CopyFrom(options);
    }
}
