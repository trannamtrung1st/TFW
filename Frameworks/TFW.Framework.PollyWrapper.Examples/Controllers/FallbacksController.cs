using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Fallback;
using Polly.Registry;
using System;
using System.Net.Http;
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
