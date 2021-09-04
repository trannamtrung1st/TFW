using Microsoft.AspNetCore.Http;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Web.Helpers;

namespace TFW.Docs.Cross.Extensions
{
    public static class HttpContextExtensions
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
