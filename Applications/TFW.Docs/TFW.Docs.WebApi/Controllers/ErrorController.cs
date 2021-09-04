using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Serilog;
using System;
using System.Threading.Tasks;
using TFW.Docs.Business;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Exceptions;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Web.Features;

namespace TFW.Docs.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(Routing.Controller.Error.Route)]
    public class ErrorController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;

        public ErrorController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCode> resultLocalizer,
            IWebHostEnvironment env) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _env = env;
        }

        [Route(Routing.Controller.Error.HandleException)]
        public async Task<IActionResult> HandleException()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = context.Error;

            if (ex == null) return BadRequest();

            AppResult response = null;

            if (ex is AppValidationException validEx)
            {
                return BadRequest(validEx.Result);
            }
            else if (ex is DbUpdateException dbEx)
            {
                response = ParseDbUpdateExceptionResult(dbEx);
            }
            else if (ex is AuthorizationException authEx)
            {
                if (authEx.IsForbidden)
                    return Forbid();

                return Unauthorized();
            }
            else if (ex is AppException appEx)
            {
                response = appEx.Result;
            }

            if (response == null)
            {
                if (_env.IsDevelopment())
                    response = AppResult.Error(resultLocalizer, data: ex);
                else response = AppResult.Error(resultLocalizer);

                await LogErrorRequestAsync(ex);
            }

            return Error(response);
        }

        private AppResult ParseDbUpdateExceptionResult(DbUpdateException ex)
        {
            AppResult appResult = null;
            var method = Request.Method;

            if (HttpMethods.IsDelete(method))
                appResult = AppResult.DependencyDeleteFail(resultLocalizer);

            return appResult;
        }

        private async Task LogErrorRequestAsync(Exception ex)
        {
            var originalRequest = HttpContext.Features.Get<ISimpleHttpRequestFeature>();
            var bodyInfo = "";

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