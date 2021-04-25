using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Docs.ApiClient
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClient(this IServiceCollection services, string baseUrl)
        {
            return services.AddSingleton<IApiClient>(_ =>
            {
                return new ApiClient(baseUrl);
            });
        }
    }
}
