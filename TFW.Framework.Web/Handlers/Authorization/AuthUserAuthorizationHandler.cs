using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Web.Requirements;

namespace TFW.Framework.Web.Handlers
{
    public class AuthUserAuthorizationHandler : AuthorizationHandler<AuthUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthUserRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated &&
                (requirement.Role == null || context.User.IsInRole(requirement.Role)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
