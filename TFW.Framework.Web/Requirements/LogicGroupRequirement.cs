using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Requirements
{
    public class LogicGroupRequirement : IAuthorizationRequirement
    {
        public const string PolicyName = "LogicGroup";
    }
}
