using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Caching;
using Polly.Contrib.WaitAndRetry;
using Polly.Fallback;
using Polly.Registry;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FallbacksController : ControllerBase
    {
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public FallbacksController(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        [HttpGet("fallback")]
        public async Task<object> Fallback()
        {
            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var result = await _policyRegistry.Get<AsyncFallbackPolicy<string>>(Startup.FallbackPolicy).ExecuteAsync(async (context) =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/always-fail");
                var resp = await client.GetStringAsync(uri);
                return resp;
            }, new Context());

            return result;
        }

    }
}
