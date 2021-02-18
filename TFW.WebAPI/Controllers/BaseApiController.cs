﻿using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;

namespace TFW.WebAPI.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly DbContext dbContext;

        protected T Service<T>()
        {
            return HttpContext.RequestServices.GetRequiredService<T>();
        }

        protected IActionResult Error(object obj = default)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, obj);
        }

        protected string GetAuthorityLeftPart()
        {
            return new Uri(Request.GetEncodedUrl()).GetLeftPart(UriPartial.Authority);
        }

        protected IActionResult FailValidation(AppValidationException exception)
        {
            return BadRequest(exception.Result);
        }

        protected IActionResult Success(object data)
        {
            return Ok(AppResult.Success(data));
        }

        protected IActionResult Success()
        {
            return Ok(AppResult.Success());
        }
    }
}
