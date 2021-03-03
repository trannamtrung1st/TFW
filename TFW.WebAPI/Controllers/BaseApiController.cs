using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using TFW.Cross.Models.Common;
using TFW.Data;

namespace TFW.WebAPI.Controllers
{
    [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(AppResult<ValidationData>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, null, typeof(AppResult))]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IUnitOfWork unitOfWork;

        public BaseApiController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

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
