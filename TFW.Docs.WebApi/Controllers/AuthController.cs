using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Exceptions;
using TFW.Docs.Cross.Models.Identity;
using TFW.Data;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Web.Attributes;
using TFW.Docs.WebApi.Filters;
using TFW.Docs.Cross.Providers;
using Microsoft.Extensions.Localization;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(ApiEndpoint.Auth)]
    public class AuthController : BaseApiController
    {
        public static class Endpoint
        {
            public const string RequestToken = "token";
        }

        private readonly IIdentityService _identityService;

        public AuthController(IBusinessContextProvider contextProvider, IStringLocalizer<ResultCodeResources> resultLocalizer,
            IIdentityService identityService) : base(contextProvider, resultLocalizer)
        {
            _identityService = identityService;
        }

        #region OAuth
        /// <summary>
        /// OAuth2 Token endpoint 
        /// </summary>
        /// <param name="model">OAuth2 Grant request model</param>
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
