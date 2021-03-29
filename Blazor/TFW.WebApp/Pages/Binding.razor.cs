using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.WebApp.Pages
{
    public partial class Binding
    {
        [Inject]
        private AppState AppState { get; set; }

        private string MyName { get; set; }

        protected override Task OnInitializedAsync()
        {
            MyName = "Unknown";
            return Task.CompletedTask;
        }
    }
}
