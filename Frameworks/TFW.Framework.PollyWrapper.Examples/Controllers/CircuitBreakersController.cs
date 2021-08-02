using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.CircuitBreaker;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TFW.Framework.PollyWrapper.Examples.CircuitBreakers;

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircuitBreakersController : ControllerBase
    {
        private readonly IPolicyManager _policyManager;

        public CircuitBreakersController(IPolicyManager policyManager)
        {
            _policyManager = policyManager;
        }

        [HttpGet("circuit-breaker-wrap")]
        public Task<string> CircuitBreakerWrap()
        {
            return _policyManager.GetHeavyResourcesBreaker.ExecuteAsync(() =>
            {
                var client = new HttpClient();
                var host = HttpContext.Request.Host.Value;
                var scheme = HttpContext.Request.Scheme;

                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/always-fail");
                var resp = client.GetStringAsync(uri);
                return resp;
            });
        }

        [HttpGet("advanced-circuit-breaker-wrap")]
        public Task<string> AdvancedCircuitBreakerWrap()
        {
            return _policyManager.AdvancedGetHeavyResourcesBreaker.ExecuteAsync(() =>
            {
                var client = new HttpClient();
                var host = HttpContext.Request.Host.Value;
                var scheme = HttpContext.Request.Scheme;

                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/always-fail");
                var resp = client.GetStringAsync(uri);
                return resp;
            });
        }

        [HttpGet("toggle-circuit-breaker")]
        public void ToggleCircuitBreaker()
        {
            var breaker = _policyManager.GetHeavyResourcesBreaker
                .GetPolicy<AsyncCircuitBreakerPolicy>();

            if (breaker.CircuitState != CircuitState.Isolated)
            {
                breaker.Isolate();
            }
            else
            {
                breaker.Reset();
            }
        }
    }
}
