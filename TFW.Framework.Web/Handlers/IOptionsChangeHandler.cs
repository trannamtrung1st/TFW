using System;

namespace TFW.Framework.Web.Handlers
{
    public interface IOptionsChangeHandler<TOptions>
    {
        Action<TOptions, string> OnChangeAction { get; }
    }
}
