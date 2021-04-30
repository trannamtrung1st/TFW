using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Areas.Admin.Pages.Login
{
    public class IndexModel : BasePageModel<IndexModel>, ILayoutPageModel
    {
        public static class Resources
        {
            public const string InitSuccess = nameof(InitSuccess);
            public const string ConfirmInit = nameof(ConfirmInit);
        }

        private readonly IMemoryCache _memoryCache;

        public IndexModel(IStringLocalizer<IndexModel> localizer,
            IMemoryCache memoryCache) : base(localizer)
        {
            _memoryCache = memoryCache;
        }

        public bool Init { get; set; }
        [BindProperty(Name = WebAppConsts.Admin.ReturnUrlParameter)]
        public string ReturnUrl { get; set; } = Routing.Admin.Index;

        public IActionResult OnGet(
            [FromQuery(Name = "iS")] bool? initSuccess = null)
        {
            if (User.Identity.IsAuthenticated)
                return LocalRedirect(ReturnUrl);

            if (initSuccess != null)
            {
                _memoryCache.Set(CachingKeys.InitStatus, initSuccess.Value);
                return LocalRedirect(ReturnUrl);
            }

            Init = !_memoryCache.Get<bool>(CachingKeys.InitStatus);

            return Page();
        }
    }
}
