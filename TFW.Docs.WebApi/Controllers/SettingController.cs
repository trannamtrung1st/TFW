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
using TFW.Docs.Cross.Models.Setting;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Web.Attributes;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(Routing.Controller.Setting.Route)]
    [Authorize(RoleName.Administrator)]
    public class SettingController : BaseApiController
    {
        private readonly ISettingService _settingService;

        public SettingController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCodeResources> resultLocalizer,
            ISettingService settingService) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _settingService = settingService;
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPatch(Routing.Controller.Setting.ChangeSmtpOption)]
        public async Task<IActionResult> ChangeSmtpOption(ChangeSmtpOptionModel model)
        {
            await _settingService.ChangeSmtpOptionAsync(model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPost(Routing.Controller.Setting.ReloadConfiguration)]
        public IActionResult ReloadConfiguration()
        {
            _settingService.ReloadConfiguration();

            return NoContent();
        }
    }
}
