using Microsoft.AspNetCore.Authorization;

namespace TFW.Framework.Web.Requirements
{
    public interface IGroupRequirement : IAuthorizationRequirement
    {
        public string Group { get; }
    }
}
