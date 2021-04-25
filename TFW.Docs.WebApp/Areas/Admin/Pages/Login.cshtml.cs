using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Areas.Admin.Pages
{
    public class LoginModel : BasePageModel<LoginModel>, ILayoutPageModel
    {
        private readonly IMemoryCache _memoryCache;

        public LoginModel(IStringLocalizer<LoginModel> localizer,
            IMemoryCache memoryCache) : base(localizer)
        {
            _memoryCache = memoryCache;
        }

        public bool Init { get; set; }

        public IActionResult OnGet(string returnUrl = Routing.Admin.Index)
        {
            if (User.Identity.IsAuthenticated)
                return LocalRedirect(returnUrl);

            Init = !_memoryCache.Get<bool>(CachingKeys.InitStatus);

            return Page();
        }
    }
}
