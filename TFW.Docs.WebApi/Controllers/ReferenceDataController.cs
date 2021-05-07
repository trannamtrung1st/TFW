using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Docs.Business.Services;
using TFW.Docs.Cross;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Web.Attributes;
using TFW.Docs.Cross.Providers;
using Microsoft.Extensions.Localization;
using TFW.Docs.Business;

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

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<TimeZoneOption>>))]
        [HttpGet(Routing.Controller.Reference.GetTimeZoneOptions)]
        public async Task<IActionResult> GetTimeZoneOptions()
        {
            var data = await _referenceDataService.GetTimeZoneOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<CultureOption>>))]
        [HttpGet(Routing.Controller.Reference.GetCultureOptions)]
        public async Task<IActionResult> GetCultureOptions()
        {
            var data = await _referenceDataService.GetCultureOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<CurrencyOption>>))]
        [HttpGet(Routing.Controller.Reference.GetCurrencyOptions)]
        public async Task<IActionResult> GetCurrencyOptions()
        {
            var data = await _referenceDataService.GetCurrencyOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<RegionOption>>))]
        [HttpGet(Routing.Controller.Reference.GetRegionOptions)]
        public async Task<IActionResult> GetRegionOptions()
        {
            var data = await _referenceDataService.GetRegionOptionsAsync();

            return Success(data);
        }
    }
}
