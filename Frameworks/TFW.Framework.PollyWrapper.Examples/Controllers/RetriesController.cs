using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetriesController : ControllerBase
    {
        [HttpGet("wait-and-retry")]
        public async Task<string> WaitAndRetry()
        {
            AsyncRetryPolicy<string> retry = Policy<string>
              .Handle<HttpRequestException>()
              .WaitAndRetryAsync(3, (count) => TimeSpan.FromSeconds(count * 2),
              (action, delay, count, context) =>
              {
                  Console.WriteLine($"Retry: {count} - {delay.TotalSeconds} seconds");
              });

            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var result = await retry.ExecuteAsync(() =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/always-fail");
                var resp = client.GetStringAsync(uri);
                return resp;
            });

            return result;
        }
    }
}
