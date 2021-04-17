using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Common.Extensions;

namespace TFW.Framework.Web.Attributes
{
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeAttribute(params string[] roles)
        {
            if (!roles.IsNullOrEmpty())
                Roles = string.Join(',', roles);
        }
    }
}
