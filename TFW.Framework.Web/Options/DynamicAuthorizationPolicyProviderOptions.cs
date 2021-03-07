using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Options
{
    public class DynamicAuthorizationPolicyProviderOptions
    {
        public const char DefaultSeperator = ',';

        public char Seperator { get; set; }
        public IDictionary<string, Action<string[], AuthorizationPolicyBuilder>> Providers { get; }
        public IList<string> DefaultSchemes { get; set; }

        public DynamicAuthorizationPolicyProviderOptions()
        {
            Seperator = DefaultSeperator;
            Providers = new Dictionary<string, Action<string[], AuthorizationPolicyBuilder>>();
        }
    }
}
