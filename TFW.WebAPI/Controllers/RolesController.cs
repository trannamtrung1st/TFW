using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Models.AppRole;
using TFW.Cross.Models.Common;
using TFW.Data;

namespace TFW.WebAPI.Controllers
{
    [Route(ApiEndpoint.RoleApi)]
    [Authorize]
    public class RolesController : BaseApiController
    {
        public static class Endpoint
        {
            public const string GetAllRoles = "";
        }

        private readonly IIdentityService _identityService;

        public RolesController(IUnitOfWork unitOfWork, IIdentityService identityService) : base(unitOfWork)
        {
            _identityService = identityService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<GetListRolesResponseModel>>))]
        [HttpGet(Endpoint.GetAllRoles)]
        public async Task<IActionResult> GetAllRoles()
        {
            var data = await _identityService.GetListRolesAsync();

            return Success(data);
        }
    }
}
