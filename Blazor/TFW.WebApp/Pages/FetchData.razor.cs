using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TFW.WebApp.Pages
{
    public partial class FetchData
    {
        [Inject]
        private HttpClient Http { get; set; }

        private WeatherForecast[] forecasts;
        private List<string> randomIds;
        private CancellationTokenSource renderingCancellation;

        protected override async Task OnInitializedAsync()
        {
            renderingCancellation = new CancellationTokenSource();
            randomIds = new List<string>();

            var renderIdsTask = RenderIds();

            forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");

        }

        public class WeatherForecast
        {
            public DateTime Date { get; set; }

            public int TemperatureC { get; set; }

            public string Summary { get; set; }

            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }

        protected void StopRendering()
        {
            renderingCancellation.Cancel();
        }

        protected async Task RenderIds()
        {
            await foreach (var id in GetRandomIds())
            {
                randomIds.Add(id);
                this.StateHasChanged();
            }
        }

        protected async IAsyncEnumerable<string> GetRandomIds()
        {
            for (var i = 0; i < 1000 && !renderingCancellation.IsCancellationRequested; i++)
            {
                await Task.Delay(1000, cancellationToken: renderingCancellation.Token);
                yield return Guid.NewGuid().ToString();
            }
        }
    }
}
