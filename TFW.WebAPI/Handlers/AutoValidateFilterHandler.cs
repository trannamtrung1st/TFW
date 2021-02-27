using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Cross.Models.Common;
using TFW.Framework.AutoMapper.Helpers;
using TFW.Framework.DI.Attributes;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Web.Attributes;
using TFW.Framework.Web.Helpers;

namespace TFW.WebAPI.Handlers
{
    public interface IAutoValidateFilterHandler
    {
        void OnActionExecuted(ActionExecutedContext context, object filter);
        void OnActionExecuting(ActionExecutingContext context, object filter);
        Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next, object filter);
    }

    [SingletonService(ServiceType = typeof(IAutoValidateFilterHandler))]
    public class AutoValidateFilterHandler : IAutoValidateFilterHandler
    {
        public void OnActionExecuted(ActionExecutedContext context, object filter)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context, object filter)
        {
            if (context.ModelState.IsValid || filter.ShouldSkip(context)) return;

            var resultProvider = context.HttpContext.RequestServices.GetRequiredService<IValidationResultProvider>();

            var results = resultProvider.Results
                .Where(o => !o.IsValid).SelectMany(o => o.Errors).MapTo<AppResult>().ToArray();

            var validationData = new ValidationData();
            validationData.Fail(results);

            var appResult = AppResult.FailValidation(validationData, validationData.Message);

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
