﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using TFW.Docs.Business;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Web.Attributes;
using AllowAnonymous = Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(Routing.Controller.User.Route)]
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IIdentityService _identityService;

        public UserController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCode> resultLocalizer,
            IIdentityService identityService) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _identityService = identityService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<ListResponseModel<ListAppUserModel>>))]
        [HttpGet(Routing.Controller.User.GetListAppUser)]
        public async Task<IActionResult> GetListAppUser([FromQuery] ListAppUserRequestModel model)
        {
            var data = await _identityService.GetListAppUserAsync<ListAppUserModel>(model);

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<int>))]
        [HttpGet(Routing.Controller.User.GetTotalUserCount)]
        public async Task<IActionResult> GetTotalUserCount()
        {
            var count = await _identityService.GetTotalUserCountAsync();

            return Success(count);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<UserProfileModel>))]
        [HttpGet(Routing.Controller.User.GetCurrentUserProfile)]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            var data = await _identityService.GetUserProfileAsync(UserId);

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPost(Routing.Controller.User.Init)]
        [AllowAnonymous]
        public async Task<IActionResult> Initialize([FromForm] RegisterModel model)
        {
            await _identityService.InitializeAsync(model);

            return NoContent();
        }

#if DEBUG
        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPost(Routing.Controller.User.Register)]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {
            await _identityService.RegisterAsync(model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPost(Routing.Controller.User.AddUserRoles)]
        public async Task<IActionResult> AddUserRoles(ChangeUserRolesBaseModel model)
        {
            await _identityService.AddUserRolesAsync(model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpDelete(Routing.Controller.User.RemoveUserRoles)]
        public async Task<IActionResult> RemoveUserRoles(ChangeUserRolesBaseModel model)
        {
            await _identityService.RemoveUserRolesAsync(model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<ListResponseModel<ListAppUserModel>>))]
        [HttpGet(Routing.Controller.User.GetListDeletedAppUser)]
        [AllowAnonymous]
        public async Task<IActionResult> GetListDeletedAppUser()
        {
            var data = await _identityService.GetListDeletedAppUserAsync<ListAppUserModel>();

            return Success(data);
        }
#endif
    }
}
