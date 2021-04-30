using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Pages
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
