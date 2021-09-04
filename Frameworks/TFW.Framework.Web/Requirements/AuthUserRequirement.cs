namespace TFW.Framework.Web.Requirements
{
    public class AuthUserRequirement : IGroupRequirement
    {
        public string Group { get; }
        public string Role { get; }

        public AuthUserRequirement(string role = null, string group = null)
        {
            Role = role;
            Group = group;
        }
    }
}
