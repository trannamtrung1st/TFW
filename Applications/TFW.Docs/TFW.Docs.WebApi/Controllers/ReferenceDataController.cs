﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using TFW.Docs.Business;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Docs.Cross.Providers;
using TFW.Framework.Web.Attributes;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(Routing.Controller.Reference.Route)]
    [Authorize]
    public class ReferenceDataController : BaseApiController
    {
        private readonly IReferenceDataService _referenceDataService;

        public ReferenceDataController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider,
            IStringLocalizer<ResultCode> resultLocalizer,
            IReferenceDataService referenceDataService) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _referenceDataService = referenceDataService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<ListResponseModel<TimeZoneOption>>))]
        [HttpGet(Routing.Controller.Reference.GetTimeZoneOptions)]
        public async Task<IActionResult> GetTimeZoneOptions()
        {
            var data = await _referenceDataService.GetTimeZoneOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<ListResponseModel<CultureOption>>))]
        [HttpGet(Routing.Controller.Reference.GetCultureOptions)]
        public async Task<IActionResult> GetCultureOptions()
        {
            var data = await _referenceDataService.GetCultureOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<ListResponseModel<CurrencyOption>>))]
        [HttpGet(Routing.Controller.Reference.GetCurrencyOptions)]
        public async Task<IActionResult> GetCurrencyOptions()
        {
            var data = await _referenceDataService.GetCurrencyOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<ListResponseModel<RegionOption>>))]
        [HttpGet(Routing.Controller.Reference.GetRegionOptions)]
        public async Task<IActionResult> GetRegionOptions()
        {
            var data = await _referenceDataService.GetRegionOptionsAsync();

            return Success(data);
        }
    }
}
