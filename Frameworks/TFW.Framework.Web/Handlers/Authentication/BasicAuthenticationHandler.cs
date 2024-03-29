﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TFW.Framework.Web.Options;

namespace TFW.Framework.Web.Handlers
{
    // [TODO] Handle 'realm' parameter
    public abstract class BasicAuthenticationHandler<TOptions> : AuthorizationHeaderHandler<TOptions>
        where TOptions : BasicAuthenticationOptions, new()
    {
        public BasicAuthenticationHandler(IOptionsMonitor<TOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult authResult = null;

            try
            {
                var authHeader = GetAuthorizationHeader();

                if (authHeader != null)
                {
                    if (authHeader.Scheme.Equals(Scheme.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8
                            .GetString(Convert.FromBase64String(authHeader.Parameter ?? string.Empty))
                            .Split(':', 2);

                        if (credentials.Length == 2)
                        {
                            var ticket = await AuthenticateAsync(credentials[0], credentials[1]);

                            if (ticket != null)
                                authResult = AuthenticateResult.Success(ticket);
                            else authResult = AuthenticateResult.Fail("Unauthorized");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                authResult = AuthenticateResult.Fail(ex);
            }

            if (authResult == null) authResult = AuthenticateResult.NoResult();

            return authResult;
        }

        public abstract Task<AuthenticationTicket> AuthenticateAsync(string username, string password);
    }
}
