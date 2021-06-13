using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpecFlowCalculator.Specs
{
    public class SingleonService
    {
        public DateTime Init { get; } = DateTime.Now;
    }

    public static class Services
    {
        [ScenarioDependencies]
        public static IServiceCollection Create()
        {
            var services = new ServiceCollection();

            services.AddSingleton<SingleonService>();

            return services;
        }
    }
}
