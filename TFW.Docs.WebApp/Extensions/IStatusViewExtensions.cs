using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.WebApp.Pages.Shared;

namespace TFW.Docs.WebApp.Extensions
{
    public static class IStatusViewExtensions
    {
        public static IActionResult StatusViewAdmin<T>(this T model) where T : PageModel, IStatusPage
        {
            return new ViewResult()
            {
                TempData = model.TempData,
                ViewData = model.ViewData,
                ViewName = AppViews.Admin.Status
            };
        }
    }
}
