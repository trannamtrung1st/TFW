using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Common.Helpers;
using TFW.Framework.Web.Options;

namespace TFW.Framework.Web.Providers
{
    public class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _authOptions;
        private readonly DynamicAuthorizationPolicyProviderOptions _dynamicOptions;
        private readonly IAuthorizationPolicyProvider _fallbackProvider;

        public DynamicAuthorizationPolicyProvider(IOptions<AuthorizationOptions> authOptions,
            IOptions<DynamicAuthorizationPolicyProviderOptions> dynamicOptions)
        {
            _authOptions = authOptions.Value;
            _dynamicOptions = dynamicOptions.Value;
            _fallbackProvider = new DefaultAuthorizationPolicyProvider(authOptions);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return _fallbackProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return _fallbackProvider.GetFallbackPolicyAsync();
        }

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.Contains(_dynamicOptions.Seperator))
                return await _fallbackProvider.GetPolicyAsync(policyName);

            var parts = policyName.Split(_dynamicOptions.Seperator);

            if (parts.Length <= 1)
                throw new InvalidOperationException("Not any param is specified");

            var paramList = parts.SubSet(1, parts.Length - 1);

            if (!_dynamicOptions.Providers.ContainsKey(parts[0]))
                throw new InvalidOperationException($"No provider for policy '{parts[0]}'");

            var builder = new AuthorizationPolicyBuilder();

            _dynamicOptions.Providers[parts[0]](paramList, builder);

            if (builder.AuthenticationSchemes?.Any() != true)
                builder.AuthenticationSchemes = _dynamicOptions.DefaultSchemes;

            return builder.Build();
        }
    }
}
