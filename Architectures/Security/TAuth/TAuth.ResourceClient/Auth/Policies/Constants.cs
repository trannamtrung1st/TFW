using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAuth.ResourceClient.Auth.Policies
{
    public static class PolicyNames
    {
        public static class Resource
        {
            public const string CanCreateResource = nameof(CanCreateResource);
        }

        public const string IsAdmin = nameof(IsAdmin);
        public const string EmailVerified = nameof(EmailVerified);
    }
}
