﻿using System;
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
using TFW.Framework.EFCore.Context;

namespace TFW.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route(ApiEndpoint.Error)]
    [ApiController]
    public class ErrorController : BaseApiController
    {
        private readonly IWebHostEnvironment _env;

        public ErrorController(IHighLevelDbContext dbContext, IWebHostEnvironment env) : base(dbContext)
        {
            _env = env;
        }

        [Route("")]
        public IActionResult HandleException()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error == null) return BadRequest();

            var e = context.Error;
            // logging ...
            AppResult response;

            if (e is AppException)
                response = (e as AppException).Result;
            else
            {
                if (_env.IsDevelopment())
                    response = AppResult.Error(data: e);
                else response = AppResult.Error();
            }

            return Error(response);
        }
    }
}