using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TFW.Cross.Models.Exceptions
{
    public class AuthorizationException : Exception
    {
        private HttpStatusCode _statusCode;

        private AuthorizationException(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        public bool IsUnauthorized => _statusCode == HttpStatusCode.Unauthorized;
        public bool IsForbidden => _statusCode == HttpStatusCode.Forbidden;

        public static AuthorizationException Unauthorized()
        {
            return new AuthorizationException(HttpStatusCode.Unauthorized);
        }

        public static AuthorizationException Forbidden()
        {
            return new AuthorizationException(HttpStatusCode.Forbidden);
        }
    }
}
