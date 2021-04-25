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
using TFW.Data;
using TFW.Framework.Web.Attributes;
using TFW.Docs.Cross.Providers;
using Microsoft.Extensions.Localization;
using TFW.Docs.Business;

namespace TFW.Docs.WebApi.Controllers
{
    [Route(ApiEndpoint.ReferenceDataApi)]
    [Authorize]
    public class ReferenceDataController : BaseApiController
    {
        public static class Endpoint
        {
            public const string GetTimeZoneOptions = "time-zones";
            public const string GetCultureOptions = "cultures";
            public const string GetCurrencyOptions = "currencies";
            public const string GetRegionOptions = "regions";
        }

        private readonly IReferenceDataService _referenceDataService;

        public ReferenceDataController(IUnitOfWork unitOfWork,
            IBusinessContextProvider contextProvider, IStringLocalizer<ResultCodeResources> resultLocalizer,
            IReferenceDataService referenceDataService) : base(unitOfWork, contextProvider, resultLocalizer)
        {
            _referenceDataService = referenceDataService;
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<TimeZoneOption>>))]
        [HttpGet(Endpoint.GetTimeZoneOptions)]
        public async Task<IActionResult> GetTimeZoneOptions()
        {
            var data = await _referenceDataService.GetTimeZoneOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<CultureOption>>))]
        [HttpGet(Endpoint.GetCultureOptions)]
        public async Task<IActionResult> GetCultureOptions()
        {
            var data = await _referenceDataService.GetCultureOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<CurrencyOption>>))]
        [HttpGet(Endpoint.GetCurrencyOptions)]
        public async Task<IActionResult> GetCurrencyOptions()
        {
            var data = await _referenceDataService.GetCurrencyOptionsAsync();

            return Success(data);
        }

        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(AppResult<GetListResponseModel<RegionOption>>))]
        [HttpGet(Endpoint.GetRegionOptions)]
        public async Task<IActionResult> GetRegionOptions()
        {
            var data = await _referenceDataService.GetRegionOptionsAsync();

            return Success(data);
        }
    }
}
