using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Caching;
using Polly.Registry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CachesController : ControllerBase
    {
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public CachesController(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        [HttpGet("cache")]
        public async Task<object> Cache()
        {
            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var result = await _policyRegistry.Get<AsyncCachePolicy>(Startup.CachePolicy).ExecuteAsync(async (context) =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/timeout");
                var resp = await client.GetStringAsync(uri);
                return resp;
            }, new Context(nameof(Cache)));

            return result;
        }

    }
}
