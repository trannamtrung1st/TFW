using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.ConsoleApp;

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

            using var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(apiUrl)
            };

            var tasks = new List<Task>();

            var sw = Stopwatch.StartNew();

            for (var i = 0; i < numberRequest; i++)
            {
                var postTask = httpClient.PostAsync("/auth/token", new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>
                    {
                    }));

                tasks.Add(postTask);
            }

            await Task.WhenAll(tasks);

            sw.Stop();
            Console.WriteLine("Total: {0}", sw.ElapsedMilliseconds);

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
        private const int DefaultNumOfRequest = 200;
    }
}
