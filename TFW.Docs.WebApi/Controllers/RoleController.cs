using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TFW.Docs.Business;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.AppRole;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Web.Attributes;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(Routing.Controller.Role.Route)]
    [Authorize]
    public class RoleController : BaseApiController
    {
        private readonly IIdentityService _identityService;

        public RoleController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCodeResources> resultLocalizer,
            IIdentityService identityService) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _identityService = identityService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<GetListRolesResponseModel>>))]
        [HttpGet(Routing.Controller.Role.GetAllRoles)]
        public async Task<IActionResult> GetAllRoles()
        {
            var data = await _identityService.GetListRolesAsync<GetListRolesResponseModel>();

            return Success(data);
        }
    }
}
