using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Handlers
{
    public abstract class BaseAuthenticationHandler<TOptions> : AuthenticationHandler<TOptions>
        where TOptions : AuthenticationSchemeOptions, new()
    {
        protected BaseAuthenticationHandler(IOptionsMonitor<TOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            await base.HandleChallengeAsync(properties);

            if (Response.StatusCode == 401)
            {
                var wwwAuth = Response.Headers.GetCommaSeparatedValues(HeaderNames.WWWAuthenticate);
                if (wwwAuth == null || !wwwAuth.Contains(Scheme.Name))
                {
                    Response.Headers.Append(HeaderNames.WWWAuthenticate, Scheme.Name);
                }
            }
        }
    }

    public abstract class AuthorizationHeaderHandler<TOptions> : BaseAuthenticationHandler<TOptions>
        where TOptions : AuthenticationSchemeOptions, new()
    {
        protected AuthorizationHeaderHandler(IOptionsMonitor<TOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        public virtual AuthenticationHeaderValue GetAuthorizationHeader()
        {
            var authHeaderStr = Request.Headers[HeaderNames.Authorization];
            if (string.IsNullOrEmpty(authHeaderStr)) return null;

            var authHeaderValue = AuthenticationHeaderValue.Parse(authHeaderStr);

            return authHeaderValue;
        }
    }
}
