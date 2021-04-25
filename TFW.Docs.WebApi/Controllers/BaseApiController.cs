using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using TFW.Docs.Business;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Security.Extensions;

namespace TFW.Docs.WebApi.Controllers
{
    [ApiController]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(AppResult<ValidationData>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, null, typeof(AppResult))]
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IBusinessContextProvider contextProvider;
        protected readonly IStringLocalizer<ResultCodeResources> resultLocalizer;

        public BaseApiController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCodeResources> resultLocalizer)
        {
            this.unitOfWork = unitOfWork;
            this.contextProvider = contextProvider;
            this.resultLocalizer = resultLocalizer;
        }

        protected int UserId
        {
            get
            {
                int userId;
                int.TryParse(User.IdentityName(), out userId);
                return userId;
            }
        }

        protected PrincipalInfo Principal => contextProvider.BusinessContext.PrincipalInfo;

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
            return Ok(AppResult.Success(resultLocalizer, data));
        }

        protected IActionResult Success()
        {
            return Ok(AppResult.Success(resultLocalizer));
        }
    }
}
