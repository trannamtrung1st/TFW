using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Handlers
{
    public abstract class BaseOptionsChangeHandler<TOptions> : IOptionsChangeHandler<TOptions>
    {
        protected readonly IConfigurationRoot configurationRoot;

        public BaseOptionsChangeHandler(IConfiguration configuration)
        {
            configurationRoot = (IConfigurationRoot)configuration;
        }

        public abstract Action<TOptions, string> OnChangeAction { get; }
    }
}
