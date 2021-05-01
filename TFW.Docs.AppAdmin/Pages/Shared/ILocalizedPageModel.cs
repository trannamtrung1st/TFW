using Microsoft.Extensions.Localization;

namespace TFW.Docs.AppAdmin.Pages.Shared
{
    public interface ILocalizedPageModel
    {
        IStringLocalizer Localizer { get; }
    }
}
