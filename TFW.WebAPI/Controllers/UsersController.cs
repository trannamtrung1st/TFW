using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Models;
using TFW.Framework.WebAPI.Bindings;

namespace TFW.WebAPI.Controllers
{
    [Route(ApiEndpoint.UserApi)]
    [ApiController]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAppUserList([FromQuery][QueryObject]GetAppUserListRequestModel model)
        {
            var validationData = await _identityService.ValidateGetAppUserListAsync(UserInfo, model);

            if (!validationData.IsValid)
                return FailValidation(validationData);

            var data = await _identityService.GetListAppUserAsync(model);

            return Success(data);
        }

    }
}
