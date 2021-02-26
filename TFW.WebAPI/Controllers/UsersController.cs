using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Models.AppUser;
using TFW.Data;
using TFW.Framework.WebAPI.Bindings;

namespace TFW.WebAPI.Controllers
{
    [Route(ApiEndpoint.UserApi)]
    [ApiController]
    //[Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IIdentityService _identityService;

        public UsersController(IUnitOfWork unitOfWork, IIdentityService identityService) : base(unitOfWork)
        {
            _identityService = identityService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAppUserList([FromQuery][QueryObject] GetAppUserListRequestModel model)
        {
            var data = await _identityService.GetListAppUserAsync(model);

            return Success(data);
        }


#if DEBUG
        [HttpGet("deleted")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDeletedAppUserList()
        {
            var data = await _identityService.GetListDeletedAppUserAsync();

            return Success(data);
        }
#endif

    }
}
