using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Requirements;
using TFW.Framework.Security.Extensions;

namespace TFW.Docs.WebApi.Handlers
{
    public class AdminOrOwnerAuthorizationHandler : AuthorizationHandler<AdminOrOwnerRequirement, IAppAuditableEntity>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AdminOrOwnerRequirement requirement, IAppAuditableEntity resource)
        {
            int userId;
            if (context.User.IsInRole(RoleName.Administrator)
                || (int.TryParse(context.User.IdentityName(), out userId) && userId == resource.CreatedUserId))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
