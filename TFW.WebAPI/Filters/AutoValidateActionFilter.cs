using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFW.WebAPI.Handlers;

namespace TFW.WebAPI.Filters
{
    public class AutoValidateActionFilter : IActionFilter, IAsyncActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var filterHandler = context.HttpContext.RequestServices.GetRequiredService<IAutoValidateFilterHandler>();

            filterHandler.OnActionExecuting(context, this);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var filterHandler = context.HttpContext.RequestServices.GetRequiredService<IAutoValidateFilterHandler>();

            await filterHandler.OnActionExecutionAsync(context, next, this);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var filterHandler = context.HttpContext.RequestServices.GetRequiredService<IAutoValidateFilterHandler>();

            filterHandler.OnActionExecuted(context, this);
        }
    }
}
