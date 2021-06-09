using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TFW.Framework.Web.Filters
{
    public class UseSystemJsonOutput : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            if (ctx.Result is JsonResult jsonResult)
                ctx.Result = new ObjectResult(jsonResult.Value);

            if (ctx.Result is ObjectResult objectResult)
            {
                var jsonOpt = ctx.HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>();
                var formatter = new SystemTextJsonOutputFormatter(jsonOpt.Value.JsonSerializerOptions);
                objectResult.Formatters.Add(formatter);
            }
        }
    }
}
