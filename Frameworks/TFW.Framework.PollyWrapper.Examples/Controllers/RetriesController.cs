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
                .Handle<HttpRequestException>().OrResult(default(string))
                .WaitAndRetryAsync(retryCount: 3,
                    // May use 429 response header RetryAfter to provide duration
                    sleepDurationProvider: (attempt, context) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (action, delay, count, context) =>
                    {
                        Console.WriteLine($"Retry: {count} - {delay.TotalSeconds} seconds");
                    });

            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var result = await retry.ExecuteAsync((context) =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/always-fail");
                var resp = client.GetStringAsync(uri);
                return resp;
            }, new Context());

            return result;
        }

        [HttpGet("refresh-auth")]
        public async Task<string> RefreshAuth()
        {
            var token = "wrong-token";

            var authorisationEnsuringPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(
                    retryCount: 1,
                    onRetry: (outcome, retryNumber, context) =>
                    {
                        Console.WriteLine("Refresh token");
                        token = "right-token";
                    });

            var retry = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>().OrResult(resp => resp.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(retryCount: 3,
                    sleepDurationProvider: (attempt, context) => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: (action, delay, count, context) =>
                    {
                        Console.WriteLine($"Retry: {count} - {delay.TotalSeconds} seconds");
                    });

            var wrap = retry.WrapAsync(authorisationEnsuringPolicy);
            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var response = await wrap.ExecuteAsync(context =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), $"/api/defects/need-token?token={token}");
                var resp = client.GetAsync(uri);
                return resp;
            }, new Context());

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return result;

            throw new HttpRequestException(result);
        }

        [HttpGet("jitter-retry")]
        public async Task<string> JitterRetry()
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(2), retryCount: 3);

            AsyncRetryPolicy<string> retry = Policy<string>
                .Handle<HttpRequestException>().OrResult(default(string))
                .WaitAndRetryAsync(delay,
                    onRetry: (action, delay, count, context) =>
                    {
                        Console.WriteLine($"Retry: {count} - {delay.TotalSeconds} seconds");
                    });

            var client = new HttpClient();
            var host = HttpContext.Request.Host.Value;
            var scheme = HttpContext.Request.Scheme;

            var result = await retry.ExecuteAsync((context) =>
            {
                var uri = new Uri(new Uri($"{scheme}://{host}"), "/api/defects/always-fail");
                var resp = client.GetStringAsync(uri);
                return resp;
            }, new Context());

            return result;
        }
    }
}
