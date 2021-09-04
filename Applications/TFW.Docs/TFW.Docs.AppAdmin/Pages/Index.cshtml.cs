using Microsoft.Extensions.Localization;
using TFW.Docs.AppAdmin.Pages.Shared;

namespace TFW.Docs.AppAdmin.Pages
{
    public class IndexModel : BasePageModel<IndexModel>, IPageModel
    {
        public IndexModel(IStringLocalizer<IndexModel> localizer) : base(localizer)
        {
        }

        public void OnGet()
        {
        }
    }
}
