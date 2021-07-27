using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TFW.Framework.PollyWrapper.Examples.CircuitBreakers
{
    public interface ICircuitBreakerManager
    {
        AsyncPolicyWrap GetHeavyResourcesBreaker { get; }
        AsyncPolicyWrap AdvancedGetHeavyResourcesBreaker { get; }
    }

    public class CircuitBreakerManager : ICircuitBreakerManager
    {
        public CircuitBreakerManager()
        {
            Init();
        }

        public AsyncPolicyWrap GetHeavyResourcesBreaker { get; private set; }
        public AsyncPolicyWrap AdvancedGetHeavyResourcesBreaker { get; private set; }

        private void Init()
        {
            var delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 3);

            AsyncRetryPolicy retry = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(delay,
                    onRetry: (action, delay, count, context) =>
                    {
                        Console.WriteLine($"Retry: {count} - {delay.TotalSeconds} seconds");
                    });

            var simpleBreaker = Policy
                .Handle<HttpRequestException>()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking: 3, durationOfBreak: TimeSpan.FromMinutes(1),
                    onBreak: (ex, breakTime) =>
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine($"Break for {breakTime}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine($"Reset circuit breaker named {nameof(GetHeavyResourcesBreaker)}");
                    }).WithPolicyKey(nameof(GetHeavyResourcesBreaker));

            GetHeavyResourcesBreaker = simpleBreaker.WrapAsync(retry);

            var advancedBreaker = Policy
                .Handle<HttpRequestException>()
                .AdvancedCircuitBreakerAsync(failureThreshold: 0.5,
                    samplingDuration: TimeSpan.FromMinutes(1),
                    minimumThroughput: 5,
                    durationOfBreak: TimeSpan.FromMinutes(1),
                    onBreak: (ex, breakTime) =>
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine($"Break for {breakTime}");
                    },
                    onReset: () =>
                    {
                        Console.WriteLine($"Reset circuit breaker named {nameof(GetHeavyResourcesBreaker)}");
                    }).WithPolicyKey(nameof(GetHeavyResourcesBreaker));

            AdvancedGetHeavyResourcesBreaker = advancedBreaker.WrapAsync(retry);
        }
    }
}
