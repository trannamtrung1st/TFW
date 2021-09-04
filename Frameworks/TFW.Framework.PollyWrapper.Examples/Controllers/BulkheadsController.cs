using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Bulkhead;
using Polly.Registry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkheadsController : ControllerBase
    {
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public BulkheadsController(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        [HttpGet("bulkhead")]
        public async Task<object> Bulkhead()
        {
            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var bulkhead = _policyRegistry.Get<AsyncBulkheadPolicy>(Startup.BulkheadPolicy);
            var bhCount = bulkhead.BulkheadAvailableCount;
            var queueCount = bulkhead.QueueAvailableCount;

            var result = await bulkhead.ExecuteAsync(async (context) =>
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
