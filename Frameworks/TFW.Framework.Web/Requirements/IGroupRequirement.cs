using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Requirements
{
    public interface IGroupRequirement : IAuthorizationRequirement
    {
        public string Group { get; }
    }
}
