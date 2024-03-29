﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TFW.Docs.WebApi.Handlers;

namespace TFW.Docs.WebApi.Attributes
{
    public class AutoValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var filterHandler = context.HttpContext.RequestServices.GetRequiredService<IAutoValidateFilterHandler>();

            filterHandler.OnActionExecuting(context, this);
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var filterHandler = context.HttpContext.RequestServices.GetRequiredService<IAutoValidateFilterHandler>();

            await filterHandler.OnActionExecutionAsync(context, next, this);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var filterHandler = context.HttpContext.RequestServices.GetRequiredService<IAutoValidateFilterHandler>();

            filterHandler.OnActionExecuted(context, this);
        }
    }
}
