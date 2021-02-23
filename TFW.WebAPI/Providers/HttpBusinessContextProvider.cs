using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TFW.Cross;
using TFW.Cross.Helpers;
using TFW.Cross.Models.Common;
using TFW.Cross.Providers;
using TFW.Framework.DI.Attributes;

namespace TFW.WebAPI.Providers
{
    public class HttpBusinessContextProvider : IBusinessContextProvider
    {
        public BusinessContext BusinessContext => new HttpBusinessContext();
    }

    internal class HttpBusinessContext : BusinessContext
    {
        public override PrincipalInfo PrincipalInfo => HttpContext.Current.GetPrincipalInfo();
    }
}
