using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Docs.Business;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.Cross.Providers;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(Routing.Controller.Setting.Route)]
    public class SettingController : BaseApiController
    {
        private readonly ISettingService _settingService;

        public SettingController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCode> resultLocalizer,
            ISettingService settingService) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _settingService = settingService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<bool>))]
        [HttpGet(Routing.Controller.Setting.InitStatus)]
        [Authorize(AuthenticationSchemes = SecurityConsts.ClientAuthenticationScheme)]
        public async Task<IActionResult> GetInitStatus()
        {
            var status = await _settingService.GetInitStatusAsync();

            return Success(status);
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPatch(Routing.Controller.Setting.ChangeSmtpOption)]
        [Authorize(Policy.Name.Admin)]
        public async Task<IActionResult> ChangeSmtpOption(ChangeSmtpOptionModel model)
        {
            await _settingService.ChangeSmtpOptionAsync(model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPost(Routing.Controller.Setting.ReloadConfiguration)]
        [Authorize(Policy.Name.Admin)]
        public IActionResult ReloadConfiguration()
        {
            _settingService.ReloadConfiguration();

            return NoContent();
        }
    }
}
