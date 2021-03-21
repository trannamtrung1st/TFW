using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Models.Setting;
using TFW.Data;
using TFW.Framework.Web.Attributes;

namespace TFW.WebAPI.Controllers
{
    [Route(ApiEndpoint.SettingApi)]
    [Authorize(RoleName.Administrator)]
    public class SettingsController : BaseApiController
    {
        public static class Endpoint
        {
            public const string ChangeSmtpOption = "smtp";
            public const string ReloadConfiguration = "reload";
        }

        private readonly ISettingService _settingService;

        public SettingsController(IUnitOfWork unitOfWork, ISettingService settingService) : base(unitOfWork)
        {
            _settingService = settingService;
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPatch(Endpoint.ChangeSmtpOption)]
        public async Task<IActionResult> ChangeSmtpOption(ChangeSmtpOptionModel model)
        {
            await _settingService.ChangeSmtpOptionAsync(model);

            return NoContent();
        }

        [SwaggerResponse((int)HttpStatusCode.NoContent, null)]
        [HttpPost(Endpoint.ReloadConfiguration)]
        public IActionResult ReloadConfiguration()
        {
            _settingService.ReloadConfiguration();

            return NoContent();
        }
    }
}
