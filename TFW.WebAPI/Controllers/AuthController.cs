using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Models.Exceptions;
using TFW.Cross.Models.Identity;
using TFW.Data;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Web.Attributes;
using TFW.WebAPI.Filters;

namespace TFW.WebAPI.Controllers
{
    [Route(ApiEndpoint.Auth)]
    public class AuthController : BaseApiController
    {
        public static class Endpoint
        {
            public const string RequestToken = "token";
        }

        private readonly IIdentityService _identityService;

        public AuthController(IUnitOfWork unitOfWork, IIdentityService identityService) : base(unitOfWork)
        {
            _identityService = identityService;
        }

        #region OAuth
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(TokenResponseModel))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(OAuthErrorResponse))]
        [HttpPost(Endpoint.RequestToken)]
        [ShouldSkipFilter(typeof(AutoValidateActionFilter))]
        public async Task<IActionResult> RequestToken([FromForm] RequestTokenModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var resultProvider = Service<IValidationResultProvider>();

                    var firstResult = resultProvider.Results
                        .Where(o => !o.IsValid).SelectMany(o => o.Errors).FirstOrDefault();

                    var oauthException = firstResult.CustomState as OAuthException;

                    throw oauthException;
                }

                var tokenResp = await _identityService.ProvideTokenAsync(model);

                return Ok(tokenResp);
            }
            catch (OAuthException ex)
            {
                return BadRequest(ex.ErrorResponse);
            }
        }
        #endregion
    }
}
