using TFW.Framework.Web.Options;
using TFW.Framework.Web.Requirements;

namespace TFW.Framework.Web
{
    public static class DynamicAuthorizationPolicyProviderOptionsExtensions
    {
        public static DynamicAuthorizationPolicyProviderOptions ConfigureAuthUserDynamicPolicy(
            this DynamicAuthorizationPolicyProviderOptions opt, string policyName)
        {
            opt.Providers[policyName] = (paramList, builder) =>
            {
                var role = string.IsNullOrEmpty(paramList[0]) ? null : paramList[0];

                if (paramList.Length == 1)
                    builder.AddRequirements(new AuthUserRequirement(role));
                else if (paramList.Length == 2)
                    builder.AddRequirements(new AuthUserRequirement(role, paramList[1]));
            };

            return opt;
        }
    }
}
