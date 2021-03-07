using TFW.Framework.Web.Requirements;

namespace TFW.Cross.Requirements
{
    public class AdminOrOwnerRequirement : IGroupRequirement
    {
        public string Group { get; }

        public AdminOrOwnerRequirement(string group = null)
        {
            Group = group;
        }
    }
}
