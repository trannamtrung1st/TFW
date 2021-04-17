using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TFW.WebApp.Shared;

namespace TFW.WebApp
{
    public class ApplicationState : INotifyPropertyChanged
    {
        private ISyncLocalStorageService _localStorage;
        public ApplicationState(ISyncLocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        private int incrementAmount;
        public int IncrementAmount
        {
            get => incrementAmount; set
            {
                if (incrementAmount != value)
                {
                    incrementAmount = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool CollapseNavMenu
        {
            get => _localStorage.GetItem<bool>(nameof(CollapseNavMenu));
            set => _localStorage.SetItem(nameof(CollapseNavMenu), value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<ApplicationState>();
            builder.Services.AddScoped<NavMenu.ComponentState>();

            await builder.Build().RunAsync();
        }
    }
}
