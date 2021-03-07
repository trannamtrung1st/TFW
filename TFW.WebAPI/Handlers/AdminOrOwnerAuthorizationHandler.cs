using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Cross;
using TFW.Cross.Entities;
using TFW.Cross.Requirements;
using TFW.Framework.Security.Helpers;

namespace TFW.WebAPI.Handlers
{
    public class AdminOrOwnerAuthorizationHandler : AuthorizationHandler<AdminOrOwnerRequirement, IAppAuditableEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AdminOrOwnerRequirement requirement, IAppAuditableEntity resource)
        {
            if (context.User.IsInRole(RoleName.Administrator) || context.User.IdentityName() == resource.CreatedUserId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
