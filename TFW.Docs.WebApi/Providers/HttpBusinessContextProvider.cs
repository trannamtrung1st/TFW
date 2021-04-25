using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Extensions;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Providers;

namespace TFW.Docs.WebApi.Providers
{
    public class HttpBusinessContextProvider : IBusinessContextProvider
    {
        public BusinessContext BusinessContext => new HttpBusinessContext();
    }

    internal class HttpBusinessContext : BusinessContext
    {
        public override PrincipalInfo PrincipalInfo => HttpContext.Current.GetPrincipalInfo();

        public override ClaimsPrincipal User => HttpContext.Current.User;
    }
}
