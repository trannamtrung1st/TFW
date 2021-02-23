using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Providers;

namespace TFW.Cross.Helpers
{
    public static class ConfigHelper
    {
        public static IApplicationBuilder ConfigureBusinessContext(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IBusinessContextProvider>();
            BusinessContext.Configure(provider);

            return app;
        }
    }
}
