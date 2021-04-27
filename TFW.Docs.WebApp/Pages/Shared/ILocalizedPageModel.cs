using Microsoft.Extensions.Localization;

namespace TFW.Docs.WebApp.Pages.Shared
{
    public interface ILocalizedPageModel
    {
        IStringLocalizer Localizer { get; }
    }
}
