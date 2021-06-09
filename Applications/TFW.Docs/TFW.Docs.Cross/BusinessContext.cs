using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Cross
{
    public abstract class BusinessContext
    {
        public abstract PrincipalInfo PrincipalInfo { get; }
        public abstract ClaimsPrincipal User { get; }
    }
}
