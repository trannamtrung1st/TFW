using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Data.Core.Helpers
{
    public static class ConfigHelper
    {
        public static IServiceCollection ConfigureData(this IServiceCollection services)
        {
            return services.AddScoped<DataContext>()
                //return only 1 scoped context 
                .AddScoped<DbContext>(p => p.GetRequiredService<DataContext>());
        }
    }
}
