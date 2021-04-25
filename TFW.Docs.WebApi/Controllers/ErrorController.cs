using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Exceptions;
using TFW.Docs.Cross.Models.Setting;
using TFW.Data;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Web.Features;
using TFW.Docs.Cross.Providers;
using Microsoft.Extensions.Localization;

namespace TFW.Docs.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(ApiEndpoint.Error)]
    public class ErrorController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;

        public ErrorController(IBusinessContextProvider contextProvider, IStringLocalizer<ResultCodeResources> resultLocalizer,
            IWebHostEnvironment env) : base(contextProvider, resultLocalizer)
        {
            _env = env;
        }

        [Route("")]
        public async Task<IActionResult> HandleException()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = context.Error;

            if (ex == null) return BadRequest();

            AppResult response;

            if (ex is AppValidationException)
                return BadRequest((ex as AppValidationException).Result);
            else if (ex is AuthorizationException)
            {
                var authEx = ex as AuthorizationException;

                if (authEx.IsForbidden)
                    return Forbid();

                return Unauthorized();
            }
            else if (ex is AppException)
                response = (ex as AppException).Result;
            else
            {
                if (_env.IsDevelopment())
                    response = AppResult.Error(resultLocalizer, data: ex);
                else response = AppResult.Error(resultLocalizer);
            }

            await LogErrorRequestAsync(ex);

            return Error(response);
        }

        private async Task LogErrorRequestAsync(Exception ex)
        {
            var originalRequest = HttpContext.Features.Get<ISimpleHttpRequestFeature>();
            object bodyInfo = "";

            if (originalRequest.ContentLength > 0 &&
                originalRequest.ContentLength <= Settings.Get<SerilogSettings>().MaxBodyLengthForLogging)
            {
                var bodyRaw = await originalRequest.GetBody().ReadAsStringAsync();
                bodyInfo = string.IsNullOrEmpty(bodyRaw) ? "" : $"\r\n---- Raw body ----\r\n{bodyRaw}\r\n";
            }

            Log.Error(ex, "500 Response\r\nRequest: {@Request}{Body}",
                originalRequest, bodyInfo);
        }
    }
}