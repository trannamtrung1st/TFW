using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Areas.Admin.Pages
{
    public class IndexModel : BasePageModel<IndexModel>, ILayoutPageModel
    {
        public IndexModel(IStringLocalizer<IndexModel> localizer) : base(localizer)
        {
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
                return LocalRedirect(Routing.Admin.Login);

            return Page();
        }
    }
}
