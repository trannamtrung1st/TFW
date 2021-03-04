using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models.Exceptions
{
    public class OAuthException : Exception
    {
        public const string InvalidRequest = "invalid_request";
        public const string InvalidClient = "invalid_client";
        public const string InvalidGrant = "invalid_grant";
        public const string UnauthorizedClient = "unauthorized_client";
        public const string UnsupportedGrantType = "unsupported_grant_type";
        public const string InvalidScope = "invalid_scope";

        public IDictionary<string, object> ErrorResponse { get; }

        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorUri { get; set; }

        private OAuthException() { }

        public static OAuthException From(string error, string description = null, string errUri = null)
        {
            return new OAuthException
            {
                Error = error,
                ErrorDescription = description,
                ErrorUri = errUri
            };
        }
    }
}
