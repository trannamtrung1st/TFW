using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFW.Business.Services;
using TFW.Cross;
using TFW.Data;

namespace TFW.WebAPI.Controllers
{
    [Route(ApiEndpoint.ReferenceDataApi)]
    [ApiController]
    //[Authorize]
    public class ReferenceDataController : BaseApiController
    {
        public static class Endpoint
        {
            public const string GetTimeZoneOptions = "time-zones";
        }

        private readonly IReferenceDataService _referenceDataService;

        public ReferenceDataController(IUnitOfWork unitOfWork, IReferenceDataService referenceDataService) : base(unitOfWork)
        {
            _referenceDataService = referenceDataService;
        }


        [HttpGet(Endpoint.GetTimeZoneOptions)]
        public async Task<IActionResult> GetTimeZoneOptions()
        {
            var data = await _referenceDataService.GetTimeZoneOptionsAsync();

            return Success(data);
        }
    }
}
