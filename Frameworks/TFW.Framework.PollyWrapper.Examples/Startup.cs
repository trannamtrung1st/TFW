using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TFW.Framework.PollyWrapper.Examples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache()
                .AddSingleton<IAsyncCacheProvider, MemoryCacheProvider>()
                .AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
                    InitPolly(serviceProvider));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                });

                #region Bearer/Api Key
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                        Name = HeaderNames.Authorization,
                        Type = SecuritySchemeType.ApiKey
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[0]
                    }
                });
                #endregion
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public const string GetHeavyResourcesBreaker = nameof(GetHeavyResourcesBreaker);
        public const string AdvancedGetHeavyResourcesBreaker = nameof(AdvancedGetHeavyResourcesBreaker);
        public const string BulkheadPolicy = nameof(BulkheadPolicy);
        public const string CachePolicy = nameof(CachePolicy);
        public const string FallbackPolicy = nameof(FallbackPolicy);

        // High throughput scenario: https://github.com/App-vNext/Polly/wiki/Avoiding-cache-repopulation-request-storms
        private PolicyRegistry InitPolly(IServiceProvider serviceProvider)
        {
            PolicyRegistry registry = new PolicyRegistry();

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

            registry.Add(GetHeavyResourcesBreaker, simpleBreaker.WrapAsync(retry));

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

            registry.Add(AdvancedGetHeavyResourcesBreaker, advancedBreaker.WrapAsync(retry));

            registry.Add(BulkheadPolicy, Policy
                .BulkheadAsync(maxParallelization: 2,
                    maxQueuingActions: 2, onBulkheadRejectedAsync: (context) =>
                    {
                        Console.WriteLine("Rejected");
                        return Task.CompletedTask;
                    }));

            var asyncCacheProvider = serviceProvider.GetRequiredService<IAsyncCacheProvider>();

            registry.Add(CachePolicy, Policy
                .CacheAsync(asyncCacheProvider, new SlidingTtl(TimeSpan.FromMinutes(5)),
                onCacheError: (context, cacheKey, ex) =>
                {
                    Console.WriteLine($"{cacheKey} - ${ex}");
                }));

            registry.Add(FallbackPolicy,
                Policy<string>.Handle<Exception>()
                    .FallbackAsync((ct) => Task.FromResult("DEFAULT")));

            return registry;
        }
    }
}
