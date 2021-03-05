using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Cross.Models.Exceptions
{
    public class OAuthException : Exception
    {
        public static class ErrorCode
        {
            public const string InvalidRequest = "invalid_request";
            public const string InvalidClient = "invalid_client";
            public const string InvalidGrant = "invalid_grant";
            public const string UnauthorizedClient = "unauthorized_client";
            public const string UnsupportedGrantType = "unsupported_grant_type";
            public const string InvalidScope = "invalid_scope";
        }

        public OAuthErrorResponse ErrorResponse { get; private set; }

        private OAuthException() { }

        public static OAuthException From(string error, string description = null, string errorUri = null)
        {
            return new OAuthException
            {
                ErrorResponse = new OAuthErrorResponse
                {
                    Error = error,
                    ErrorDescription = description,
                    ErrorUri = errorUri
                }
            };
        }

        public static OAuthException InvalidGrant(string description = null, string errorUri = null)
        {
            return From(ErrorCode.InvalidGrant, description, errorUri);
        }

        public static OAuthException InvalidRequest(string description = null, string errorUri = null)
        {
            return From(ErrorCode.InvalidRequest, description, errorUri);
        }

        public static OAuthException InvalidClient(string description = null, string errorUri = null)
        {
            return From(ErrorCode.InvalidClient, description, errorUri);
        }

        public static OAuthException InvalidScope(string description = null, string errorUri = null)
        {
            return From(ErrorCode.InvalidScope, description, errorUri);
        }

        public static OAuthException UnauthorizedClient(string description = null, string errorUri = null)
        {
            return From(ErrorCode.UnauthorizedClient, description, errorUri);
        }

        public static OAuthException UnsupportedGrantType(string description = null, string errorUri = null)
        {
            return From(ErrorCode.UnsupportedGrantType, description, errorUri);
        }
    }
}
