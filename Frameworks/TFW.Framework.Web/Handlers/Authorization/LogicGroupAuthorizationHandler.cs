using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Web.Requirements;

namespace TFW.Framework.Web.Handlers
{
    public class LogicGroupAuthorizationHandler : AuthorizationHandler<LogicGroupRequirement>
    {
        private int _count = 0;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LogicGroupRequirement requirement)
        {
            context.Succeed(requirement);
            _count++;

            if (_count != context.Requirements.OfType<LogicGroupRequirement>().Count())
                return Task.CompletedTask;

            var grouped = context.Requirements.OfType<IGroupRequirement>()
                .GroupBy(req => req.Group);

            var isPassed = false;

            foreach (var group in grouped)
            {
                isPassed = !group.Any(req => context.PendingRequirements.Contains(req));
                if (isPassed) break;
            }

            if (isPassed)
            {
                var notSucceeded = context.PendingRequirements.OfType<IGroupRequirement>()
                    .GroupBy(req => req.Group);

                foreach (var group in notSucceeded)
                {
                    foreach (var req in group)
                        context.Succeed(req);
                }
            }

            return Task.CompletedTask;
        }
    }
}
