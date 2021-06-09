using Microsoft.Extensions.Configuration;
using System;

namespace TFW.Framework.Web.Handlers
{
    public interface IOptionsChangeHandler<TOptions>
    {
        Action<TOptions, string> OnChangeAction { get; }
    }

    public abstract class OptionsChangeHandler<TOptions> : IOptionsChangeHandler<TOptions>
    {
        protected readonly IConfigurationRoot configurationRoot;

        public OptionsChangeHandler(IConfiguration configuration)
        {
            configurationRoot = (IConfigurationRoot)configuration;
        }

        public abstract Action<TOptions, string> OnChangeAction { get; }
    }
}
