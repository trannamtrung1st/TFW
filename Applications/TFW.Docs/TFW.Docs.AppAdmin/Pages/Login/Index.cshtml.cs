using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using TFW.Docs.AppAdmin.Pages.Shared;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Exceptions;

namespace TFW.Docs.AppAdmin.Pages.Login
{
    public class IndexModel : BasePageModel<IndexModel>, IPageModel
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IStringLocalizer _resultLocalizer;

        public IndexModel(
            IStringLocalizer<IndexModel> localizer,
            IStringLocalizer<ResultCode> resultLocalizer,
            IMemoryCache memoryCache) : base(localizer)
        {
            _memoryCache = memoryCache;
            _resultLocalizer = resultLocalizer;
        }

        public bool Init { get; set; }
        public string ReturnUrl { get; set; }

        public IActionResult OnGet(
            [FromQuery(Name = "iS")] bool? initSuccess = null,
            string returnUrl = Routing.Admin.Index)
        {
            if (!Url.IsLocalUrl(returnUrl))
                throw AppValidationException.From(_resultLocalizer, ResultCode.Identity_InvalidRedirectUrl);

            if (initSuccess != null)
            {
                _memoryCache.Set(CachingKeys.InitStatus, initSuccess.Value);
                return LocalRedirect(returnUrl);
            }

            ReturnUrl = returnUrl;
            Init = !_memoryCache.Get<bool>(CachingKeys.InitStatus);

            return Page();
        }
    }
}
