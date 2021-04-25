using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Areas.Admin.Pages
{
    public class LoginModel : BasePageModel<LoginModel>, ILayoutPageModel
    {
        public LoginModel(IStringLocalizer<LoginModel> localizer) : base(localizer)
        {
        }

        public IActionResult OnGet(string returnUrl = Routing.Admin.Index)
        {
            if (User.Identity.IsAuthenticated)
                return LocalRedirect(returnUrl);

            return Page();
        }
    }
}
