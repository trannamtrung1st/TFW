using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Framework.Web.Attributes;
using TFW.Framework.Web.Options;

namespace TFW.Framework.Web.Helpers
{
    public static class FilterHelper
    {
        public static bool ShouldSkip(this object filter, ActionExecutingContext context)
        {
            var options = context.HttpContext.RequestServices
                .GetRequiredService<IOptions<FrameworkOptions>>().Value;

            var filterType = filter.GetType();
            var controllerType = context.Controller.GetType();
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            if (options.ShouldSkipFilterTypesMap.ContainsKey(descriptor.MethodInfo)
                && options.ShouldSkipFilterTypesMap[descriptor.MethodInfo].Contains(filterType))
                return true;

            return (options.ShouldSkipFilterTypesMap.ContainsKey(controllerType)
                && options.ShouldSkipFilterTypesMap[controllerType].Contains(filterType));
        }

        public static bool ShouldSkip(this object filter, ActionExecutedContext context)
        {
            var options = context.HttpContext.RequestServices
                .GetRequiredService<IOptions<FrameworkOptions>>().Value;

            var filterType = filter.GetType();
            var controllerType = context.Controller.GetType();
            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            if (options.ShouldSkipFilterTypesMap.ContainsKey(descriptor.MethodInfo)
                && options.ShouldSkipFilterTypesMap[descriptor.MethodInfo].Contains(filterType))
                return true;

            return (options.ShouldSkipFilterTypesMap.ContainsKey(controllerType)
                && options.ShouldSkipFilterTypesMap[controllerType].Contains(filterType));
        }
    }
}
