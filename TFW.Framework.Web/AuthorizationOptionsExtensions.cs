using Microsoft.AspNetCore.Authorization;
using TFW.Framework.Web.Requirements;

namespace TFW.Framework.Web
{
    public static class AuthorizationOptionsExtensions
    {
        public static AuthorizationOptions AddLogicGroup(this AuthorizationOptions opt)
        {
            opt.AddPolicy(LogicGroupRequirement.PolicyName, policy => policy.AddRequirements(new LogicGroupRequirement()));

            return opt;
        }
    }
}
