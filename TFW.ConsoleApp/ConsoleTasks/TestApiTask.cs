using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.ConsoleApp;
using TFW.Framework.Http.Helpers;

namespace TFW.ConsoleApp.ConsoleTasks
{
    public class TestApiTask : DefaultConsoleTask
    {
        public IDictionary<string, Func<Task>> Tasks => new Dictionary<string, Func<Task>>()
        {
            { $"{TestApiOpt}", TestApi },
        };

        public override string Title => "Test API";

        public override string Description => $"Options:\n" +
            $"{TestApiOpt}. {nameof(TestApi)}\n" +
            $"-----------------------------------------\n" +
            $"Input: ";

        private async Task TestApi()
        {
            Console.Clear();

            var apiUrl = XConsole.PromptLine("Api URL: ");
            if (string.IsNullOrWhiteSpace(apiUrl))
            {
                apiUrl = DefaultApiUrl;
            }

            var numberStr = XConsole.PromptLine("Number of request: ");
            int numberRequest;
            if (!int.TryParse(numberStr, out numberRequest))
                numberRequest = DefaultNumOfRequest;

            var loopStr = XConsole.PromptLine("Number of loop: ");
            int totalLoop;
            if (!int.TryParse(loopStr, out totalLoop))
                totalLoop = DefaultNumOfLoop;

            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(apiUrl)
            };

            for (var loop = 0; loop < totalLoop; loop++)
            {
                var sw = Stopwatch.StartNew();

                var tokenResp = await httpClient.PostAsFormAsync("/auth/token", new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", "admin" },
                    { "password", "123123" },
                });

                var token = await tokenResp.Content.ReadFromJsonAsync<JObject>();
                var tokenString = token.Value<string>("access_token");

                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenString);

                for (var count = 0; count < 3; count++)
                {
                    var tasks = new List<Task>();

                    for (var i = 0; i < numberRequest / 3; i++)
                    {
                        var requestTask = httpClient.GetAsync("/api/ref/time-zones");
                        tasks.Add(requestTask);
                    }

                    await Task.WhenAll(tasks);
                }

                sw.Stop();
                Console.WriteLine("Total: {0}", sw.ElapsedMilliseconds);
                Console.WriteLine("... 2s ...", sw.ElapsedMilliseconds);
                Thread.Sleep(2000);
            }

            Console.ReadLine();
            Console.Clear();
        }

        public override async Task StartAsync()
        {
            Console.Clear();

            var opt = XConsole.PromptLine(Description);

            if (Tasks.ContainsKey(opt))
                await Tasks[opt]();

            XConsole.PromptLine("\nPress enter to exit task");

            Console.Clear();
        }

        public const string TestApiOpt = "1";
        private const string DefaultApiUrl = "https://localhost:44337";
        private const int DefaultNumOfRequest = 1200;
        private const int DefaultNumOfLoop = 7;
    }
}
