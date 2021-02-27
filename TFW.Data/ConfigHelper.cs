using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Data.Providers;

namespace TFW.Data
{
    public static class ConfigHelper
    {
        public static IApplicationBuilder ConfigureUnitOfWork(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<IUnitOfWorkProvider>();
            UnitOfWorkManager.Configure(provider);

            return app;
        }
    }
}
