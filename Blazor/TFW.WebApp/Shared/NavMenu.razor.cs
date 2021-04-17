using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFW.WebApp.Shared
{
    public partial class NavMenu
    {
        [Inject]
        private ApplicationState AppState { get; set; }
        [Inject]
        private ComponentState State { get; set; }

        private void ToggleNavMenu()
        {
            State.Count++;
            AppState.CollapseNavMenu = !AppState.CollapseNavMenu;
        }

        public class ComponentState
        {
            public int Count { get; set; } = 0;
        }
    }
}
