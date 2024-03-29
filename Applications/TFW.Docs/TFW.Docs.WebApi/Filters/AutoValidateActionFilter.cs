﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TFW.Docs.WebApi.Handlers;

namespace TFW.Docs.WebApi.Filters
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
