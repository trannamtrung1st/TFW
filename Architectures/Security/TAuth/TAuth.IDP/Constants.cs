using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAuth.IDP
{
    public static class AuthConstants
    {
        public static class IIS
        {
            public const string AuthDisplayName = "Windows";
        }

        public static class AuthSchemes
        {
            public const string Facebook = nameof(Facebook);
            public const string Google = nameof(Google);
            public const string Windows = nameof(Windows);
        }
    }
}
