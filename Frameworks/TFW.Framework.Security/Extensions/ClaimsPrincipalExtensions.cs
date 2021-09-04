using System.Linq;
using System.Security.Claims;

namespace TFW.Framework.Security.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string IdentityName(this ClaimsPrincipal principal)
        {
            return principal.Identity.Name;
        }

        public static string[] Roles(this ClaimsPrincipal principal)
        {
            return principal.FindAll(ClaimTypes.Role).Select(o => o.Value).ToArray();
        }
    }
}
