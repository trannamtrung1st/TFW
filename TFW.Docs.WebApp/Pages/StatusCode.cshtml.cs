using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.WebApp.Extensions;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Pages
{
    public class StatusCodeModel : BasePageModel<StatusCodeModel>, IStatusPage
    {
        public static class Resources
        {
            public const string Message = nameof(Message);
            public const string MessageTitle = nameof(MessageTitle);
        }

        public StatusCodeModel(IStringLocalizer<StatusCodeModel> localizer) : base(localizer)
        {
        }

        public string Message => Localizer[Resources.Message];
        [BindProperty(Name = WebAppConsts.Admin.StatusParam, SupportsGet = true)]
        public int Code { get; set; }
        public string Layout { get; set; } = null;
        public string MessageTitle => Localizer[Resources.MessageTitle];
        public string StatusCodeStyle { get; set; } = "warning";
        public string OriginalUrl { get; set; }

        public IActionResult OnGet()
        {
            var statusCodeReExecuteFeature =
                HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature == null) return LocalRedirect(Routing.Root.Index);
            OriginalUrl =
                statusCodeReExecuteFeature.OriginalPathBase
                + statusCodeReExecuteFeature.OriginalPath
                + statusCodeReExecuteFeature.OriginalQueryString;

            if (statusCodeReExecuteFeature.OriginalPath.StartsWith(Routing.Admin.Index))
            {
                return this.StatusViewAdmin();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
