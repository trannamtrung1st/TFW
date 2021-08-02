using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.PollyWrapper.Examples.CircuitBreakers;

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkheadsController : ControllerBase
    {
        private readonly IPolicyManager _policyManager;

        public BulkheadsController(IPolicyManager policyManager)
        {
            _policyManager = policyManager;
        }

        [HttpGet("bulkhead")]
        public async Task<object> Bulkhead()
        {
            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var bhCount = _policyManager.BulkheadPolicy.BulkheadAvailableCount;
            var queueCount = _policyManager.BulkheadPolicy.QueueAvailableCount;

            var result = await _policyManager.BulkheadPolicy.ExecuteAsync(async (context) =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/timeout");
                var resp = await client.GetStringAsync(uri);
                return resp;
            }, new Context());

            return new
            {
                bhCount,
                queueCount
            };
        }

    }
}
