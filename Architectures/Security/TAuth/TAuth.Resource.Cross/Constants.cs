using System;
using System.Collections.Generic;
using System.Text;

namespace TAuth.Resource.Cross
{
    public static class RoleNames
    {
        public const string Administrator = nameof(Administrator);
    }

    public static class OpenIdConnectConstants
    {
        public static class PropertyNames
        {
            public const string AccessToken = "access_token";
        }

        public static class AuthSchemes
        {
            public const string Introspection = nameof(Introspection);
        }
    }
}
