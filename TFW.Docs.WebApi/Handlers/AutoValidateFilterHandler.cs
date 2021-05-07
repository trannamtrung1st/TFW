using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.AutoMapper;
using TFW.Framework.DI.Attributes;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Web.Handlers;
using TFW.Framework.Web.Helpers;

namespace TFW.Docs.WebApi.Handlers
{
    public interface IAutoValidateFilterHandler : IActionFilterHandler
    {
    }

    [SingletonService(ServiceType = typeof(IAutoValidateFilterHandler))]
    public class AutoValidateFilterHandler : IAutoValidateFilterHandler
    {
        public void OnActionExecuted(ActionExecutedContext context, object filter)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context, object filter)
        {
            if (context.ModelState.IsValid || FilterHelper.ShouldSkip(filter, context)) return;

            var resultProvider = context.HttpContext.RequestServices.GetRequiredService<IValidationResultProvider>();
            var resultLocalizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<ResultCode>>();

            var results = resultProvider.Results
                .Where(o => !o.IsValid).SelectMany(o => o.Errors).MapTo<AppResult>().ToArray();

            var validationData = new ValidationData(resultLocalizer);
            validationData.Fail(results);

            var appResult = AppResult.FailValidation(resultLocalizer, validationData, validationData.Message);

            context.Result = new BadRequestObjectResult(appResult);

            context.HttpContext.Items[nameof(AutoValidateFilterHandler)] = true;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next, object filter)
        {
            OnActionExecuting(context, filter);

            if (context.HttpContext.Items.ContainsKey(nameof(AutoValidateFilterHandler)))
                return;

            await next();
        }
    }
}
