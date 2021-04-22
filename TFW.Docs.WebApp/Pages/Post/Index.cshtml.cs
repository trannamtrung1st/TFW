using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Pages.Post
{
    public class IndexModel : BasePageModel<IndexModel>, ILayoutPageModel
    {
        public IndexModel(IStringLocalizer<IndexModel> localizer) : base(localizer)
        {
        }

        public string Title => Localizer[ResourceKeys.Title];

        public string Description => Localizer[ResourceKeys.Description];

        public void OnGet()
        {
        }
    }
}
