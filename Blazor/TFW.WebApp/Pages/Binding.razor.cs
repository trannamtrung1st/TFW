using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.WebApp.Pages
{
    public partial class Binding
    {
        [Inject]
        private ApplicationState AppState { get; set; }

        private string MyName { get; set; }

        protected override Task OnInitializedAsync()
        {
            AppState.PropertyChanged += AppState_PropertyChanged;
            MyName = "Unknown";
            return Task.CompletedTask;
        }

        private readonly string[] _usingProps = new[] { nameof(ApplicationState.IncrementAmount) };

        private void AppState_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_usingProps.Contains(e.PropertyName))
                StateHasChanged();
        }
    }
}
