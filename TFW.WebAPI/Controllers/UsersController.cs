using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Models.AppUser;
using TFW.Cross.Models.Common;
using TFW.Cross.Models.Exceptions;
using TFW.Cross.Models.Identity;
using TFW.Data;
using TFW.Framework.Web.Bindings;

namespace TFW.WebAPI.Controllers
{
    [Route(ApiEndpoint.UserApi)]
    [Authorize]
    public class UsersController : BaseApiController
    {
        public static class Endpoint
        {
            public const string GetListAppUser = "";
            public const string GetListDeletedAppUser = "deleted";
            public const string RequestToken = "/oauth/token";
        }

        private readonly IIdentityService _identityService;

        public UsersController(IUnitOfWork unitOfWork, IIdentityService identityService) : base(unitOfWork)
        {
            _identityService = identityService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<GetListAppUsersResponseModel>>))]
        [HttpGet(Endpoint.GetListAppUser)]
        public async Task<IActionResult> GetListAppUser([FromQuery][QueryObject] GetListAppUsersRequestModel model)
        {
            var data = await _identityService.GetListAppUsersAsync(model);

            return Success(data);
        }

        #region OAuth
        // [TODO] validate model
        [HttpPost(Endpoint.RequestToken)]
        public async Task<IActionResult> RequestToken([FromForm] RequestTokenModel model)
        {
            try
            {
                var tokenResp = await _identityService.ProvideTokenAsync(model);

                return Ok(tokenResp);
            }
            catch (OAuthException ex)
            {
                return BadRequest(ex.ErrorResponse);
            }
        }
        #endregion

#if DEBUG
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<GetListAppUsersResponseModel>>))]
        [HttpGet(Endpoint.GetListDeletedAppUser)]
        [AllowAnonymous]
        public async Task<IActionResult> GetListDeletedAppUser()
        {
            var data = await _identityService.GetListDeletedAppUsersAsync();

            return Success(data);
        }
#endif
    }
}
