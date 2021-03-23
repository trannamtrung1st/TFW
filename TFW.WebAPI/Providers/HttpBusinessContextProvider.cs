using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TFW.Cross;
using TFW.Cross.Helpers;
using TFW.Cross.Models.Common;
using TFW.Cross.Providers;

namespace TFW.WebAPI.Providers
{
    public class HttpBusinessContextProvider : IBusinessContextProvider
    {
        public BusinessContext BusinessContext => new HttpBusinessContext();
    }

    internal class HttpBusinessContext : BusinessContext
    {
        public override PrincipalInfo PrincipalInfo => HttpContext.Current.GetPrincipalInfo();

        public override ClaimsPrincipal User => HttpContext.Current.User;

        private IStringLocalizer _resultCodeLocalizer;
        public override IStringLocalizer ResultCodeLocalizer
        {
            get
            {
                if (_resultCodeLocalizer == null)
                    _resultCodeLocalizer = HttpContext.Current.RequestServices
                        .GetRequiredService<IStringLocalizer<ResultCodeResources>>();

                return _resultCodeLocalizer;
            }
        }
    }
}
