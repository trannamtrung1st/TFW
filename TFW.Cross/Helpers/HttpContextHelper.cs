using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Common;
using TFW.Framework.WebAPI.Helpers;

namespace TFW.Cross.Helpers
{
    public static class HttpContextHelper
    {
        public static void SetPrincipalInfo(this HttpContext context, PrincipalInfo principalInfo)
        {
            context.Items[RequestDataKey.PrincipalInfo] = principalInfo;
        }

        public static PrincipalInfo GetPrincipalInfo(this HttpContext context)
        {
            return context.GetItem<PrincipalInfo>(RequestDataKey.PrincipalInfo);
        }
    }
}
