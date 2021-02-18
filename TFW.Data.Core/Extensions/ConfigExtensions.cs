using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Data.Core.Extensions
{
    public static class ConfigExtensions
    {
        public static IServiceCollection ConfigureData(this IServiceCollection services)
        {
            return services.AddScoped<DataContext>()
                //return only 1 scoped context 
                .AddScoped<DbContext>(p => p.GetRequiredService<DataContext>());
        }
    }
}
