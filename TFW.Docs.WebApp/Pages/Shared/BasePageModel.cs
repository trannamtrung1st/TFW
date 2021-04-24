using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.Docs.WebApp.Pages.Shared
{
    public class BasePageModel<T> : PageModel, ILayoutPageModel
    {
        public BasePageModel(IStringLocalizer<T> localizer)
        {
            Localizer = localizer;
        }

        public IStringLocalizer<T> Localizer { get; }

        public virtual string Title => Localizer[ResourceKeys.Title];

        public virtual string Description => Localizer[ResourceKeys.Description];
    }
}
