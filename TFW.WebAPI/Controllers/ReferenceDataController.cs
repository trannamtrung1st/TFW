using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Cross.Models.Common;
using TFW.Data;
using TFW.Framework.Web.Attributes;

namespace TFW.WebAPI.Controllers
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

        public ReferenceDataController(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService) : base(unitOfWork)
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
