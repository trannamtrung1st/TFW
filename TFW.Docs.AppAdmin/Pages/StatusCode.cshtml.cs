using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross;
using TFW.Docs.AppAdmin.Extensions;
using TFW.Docs.AppAdmin.Pages.Shared;

namespace TFW.Docs.AppAdmin.Pages
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
        public int Code { get; set; }
        public string Layout { get; set; } = null;
        public string MessageTitle => Localizer[Resources.MessageTitle];
        public string StatusCodeStyle { get; set; } = "warning";
        public string OriginalUrl { get; set; }

        public IActionResult OnGet(int code)
        {
            var statusCodeReExecuteFeature =
                HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCodeReExecuteFeature == null) return LocalRedirect(Routing.App.Index);

            OriginalUrl =
                statusCodeReExecuteFeature.OriginalPathBase
                + statusCodeReExecuteFeature.OriginalPath
                + statusCodeReExecuteFeature.OriginalQueryString;
            Code = code;

            return this.StatusView();
        }
    }
}
