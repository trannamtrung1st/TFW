using Microsoft.AspNetCore.Authorization;

namespace TFW.Framework.Web.Requirements
{
    public class LogicGroupRequirement : IAuthorizationRequirement
    {
        public const string PolicyName = "LogicGroup";
    }
}
