using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TAuth.WorkerClient
{
    public class Program
    {
        private IServiceProvider _serviceProvider;

        private void Setup()
        {
            var services = new ServiceCollection();

            services.AddHttpClient<IWorkerService, WorkerService>(opt =>
            {
                opt.BaseAddress = new Uri("https://localhost:44357");
            }).AddClientAccessTokenHandler();

            services.AddAuthentication(opt =>
            {
                opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, opt =>
            {
                opt.Authority = "https://localhost:5001/";
                opt.ClientId = "worker-client-id";
                opt.ResponseType = OpenIdConnectResponseType.Token;
                opt.Scope.Add("resource_api.background");
                opt.ClientSecret = "worker-client-secret";

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };
            });

            // adds user and client access token management
            services.AddAccessTokenManagement(options =>
            {
                // client config is inferred from OpenID Connect settings
                // if you want to specify scopes explicitly, do it here, otherwise the scope parameter will not be sent
                options.Client.Scope = "resource_api.background";
            }).ConfigureBackchannelHttpClient()
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            }));

            _serviceProvider = services.BuildServiceProvider();
        }

        public void Start()
        {
            Setup();
            using var scope = _serviceProvider.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<IWorkerService>();
            worker.StartAsync().Wait();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            program.Start();
        }
    }

    public interface IWorkerService
    {
        Task StartAsync();
    }

    public class WorkerService : IWorkerService
    {
        private HttpClient _httpClient;

        public WorkerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                var resp = await _httpClient.GetAsync("/api/background");

                if (resp.IsSuccessStatusCode)
                {
                    var content = await resp.Content.ReadFromJsonAsync<JsonElement>();
                    Console.WriteLine(content);
                }
                else
                {
                    Console.WriteLine(resp);
                    Console.WriteLine(resp.StatusCode);
                }

                Thread.Sleep(5000);
            }
        }
    }
}
