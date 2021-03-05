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
            var data = await _identityService.GetListAppUsersAsync<GetListAppUsersResponseModel>(model);

            return Success(data);
        }

#if DEBUG
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<GetListAppUsersResponseModel>>))]
        [HttpGet(Endpoint.GetListDeletedAppUser)]
        [AllowAnonymous]
        public async Task<IActionResult> GetListDeletedAppUser()
        {
            var data = await _identityService.GetListDeletedAppUsersAsync<GetListAppUsersResponseModel>();

            return Success(data);
        }
#endif
    }
}
