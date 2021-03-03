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
    [ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {
        public static class Endpoint
        {
            public const string GetAppUserList = "";
            public const string GetDeletedAppUserList = "deleted";
        }

        private readonly IIdentityService _identityService;

        public UsersController(IUnitOfWork unitOfWork, IIdentityService identityService) : base(unitOfWork)
        {
            _identityService = identityService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<AppUserResponseModel>>))]
        [HttpGet(Endpoint.GetAppUserList)]
        public async Task<IActionResult> GetAppUserList([FromQuery][QueryObject] GetAppUserListRequestModel model)
        {
            var data = await _identityService.GetListAppUserAsync(model);

            return Success(data);
        }

#if DEBUG
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<AppUserResponseModel>>))]
        [HttpGet(Endpoint.GetDeletedAppUserList)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDeletedAppUserList()
        {
            var data = await _identityService.GetListDeletedAppUserAsync();

            return Success(data);
        }
#endif
    }
}
