using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;
using TFW.Cross;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Cross.Models.Setting;
using TFW.Data;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Web.Features;

namespace TFW.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(ApiEndpoint.Error)]
    public class ErrorController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;

        public ErrorController(IUnitOfWork unitOfWork, IWebHostEnvironment env) : base(unitOfWork)
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
                    response = AppResult.Error(data: ex);
                else response = AppResult.Error();
            }

            await LogErrorRequestAsync(ex);

            return Error(response);
        }

        private async Task LogErrorRequestAsync(Exception ex)
        {
            var originalRequest = HttpContext.Features.Get<ISimpleHttpRequestFeature>();
            object bodyInfo = "";

            if (originalRequest.ContentLength > 0 &&
                originalRequest.ContentLength <= Settings.Serilog.MaxBodyLengthForLogging)
            {
                var bodyRaw = await originalRequest.GetBody().ReadAsStringAsync();
                bodyInfo = string.IsNullOrEmpty(bodyRaw) ? "" : $"\r\n---- Raw body ----\r\n{bodyRaw}\r\n";
            }

            Log.Error(ex, "500 Response\r\nRequest: {@Request}{Body}",
                originalRequest, bodyInfo);
        }
    }
}