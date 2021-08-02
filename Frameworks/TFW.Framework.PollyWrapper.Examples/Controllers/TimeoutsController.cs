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

namespace TFW.Framework.PollyWrapper.Examples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeoutsController : ControllerBase
    {
        [HttpGet("optimistic-timeout")]
        public async Task<string> OptimisticTimeout()
        {
            var timeout = Policy.TimeoutAsync(TimeSpan.FromSeconds(7),
                Polly.Timeout.TimeoutStrategy.Optimistic, (context, time, func) =>
                {
                    Console.WriteLine($"Timeout {time}");
                    return Task.CompletedTask;
                });

            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var result = await timeout.ExecuteAsync((context, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/timeout");
                var resp = client.GetAsync(uri, cancellationToken);
                return resp;
            }, new Context(), CancellationToken.None);

            return await result.Content.ReadAsStringAsync();
        }

        [HttpGet("pestimistic-timeout")]
        public async Task<string> PestimisticTimeout()
        {
            // Using additional Task on ThreadPool (not recommended if Optimistic is possible)
            var timeout = Policy.TimeoutAsync(TimeSpan.FromSeconds(7),
                Polly.Timeout.TimeoutStrategy.Pessimistic, (context, time, func) =>
                {
                    Console.WriteLine($"Timeout {time}");
                    return Task.CompletedTask;
                });

            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var result = await timeout.ExecuteAsync((context) =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/timeout");
                var resp = client.GetStringAsync(uri);
                return resp;
            }, new Context());

            return result;
        }
    }
}
