using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.AppAdmin.Pages.Shared;

namespace TFW.Docs.AppAdmin.Extensions
{
    public static class IStatusViewExtensions
    {
        public static IActionResult StatusView<T>(this T model) where T : PageModel, IStatusPage
        {
            return new ViewResult()
            {
                TempData = model.TempData,
                ViewData = model.ViewData,
                ViewName = AppViews.Status
            };
        }
    }
}
