using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Framework.Web.Handlers;
using TFW.Framework.Web.Options;

namespace TFW.Docs.WebApi.Handlers
{
    public class AppClientAuthenticationHandler : BasicAuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly IIdentityService _identityService;

        public AppClientAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IIdentityService identityService) : base(options, logger, encoder, clock)
        {
            _identityService = identityService;
        }

        public override async Task<AuthenticationTicket> AuthenticateAsync(string username, string password)
        {
            var clientInfo = await _identityService.AuthenticateClientAsync(username, password);

            if (clientInfo == null) return null;

            var principal = _identityService.MapToClaimsPrincipal(clientInfo);
            var ticket = new AuthenticationTicket(principal, SecurityConsts.ClientAuthenticationScheme);
            return ticket;
        }
    }
}
