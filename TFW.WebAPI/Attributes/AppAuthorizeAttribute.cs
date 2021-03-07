using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Common.Helpers;

namespace TFW.WebAPI.Attributes
{
    public class AppAuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AppAuthorizeAttribute(params string[] roles)
        {
            if (!roles.IsNullOrEmpty())
                Roles = string.Join(',', roles);
        }
    }
}
