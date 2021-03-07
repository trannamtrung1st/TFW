using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Cross.Requirements;
using TFW.Framework.i18n;
using TFW.Framework.i18n.Helpers;

namespace TFW.WebAPI.Handlers
{
    public class GuestRestrictionAuthorizationHandler : AuthorizationHandler<GuestRestrictionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GuestRestrictionRequirement requirement)
        {
            var current = DateTime.UtcNow.ToTimeZoneFromUtc(Time.ThreadTimeZone);

            if (context.User.Identity.IsAuthenticated || current.TimeOfDay < requirement.MustBefore)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
