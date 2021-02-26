using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TFW.Cross;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Data;

namespace TFW.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(ApiEndpoint.Error)]
    [ApiController]
    public class ErrorController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;

        public ErrorController(IUnitOfWork unitOfWork, IWebHostEnvironment env) : base(unitOfWork)
        {
            _env = env;
        }

        [Route("")]
        public IActionResult HandleException()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = context.Error;
            
            if (ex == null) return BadRequest();

            // logging ...

            AppResult response;

            if (ex is AppValidationException)
                return FailValidation(ex as AppValidationException);
            else if (ex is AppException)
                response = (ex as AppException).Result;
            else
            {
                if (_env.IsDevelopment())
                    response = AppResult.Error(data: ex);
                else response = AppResult.Error();
            }

            return Error(response);
        }
    }
}