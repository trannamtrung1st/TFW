using Microsoft.Extensions.DependencyInjection;
using TFW.Docs.Cross.Models.Identity;

namespace TFW.Docs.ApiClient
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClient(this IServiceCollection services, string baseUrl,
            ClientInfo clientInfo = null)
        {
            return services.AddSingleton<IApiClient>(_ =>
            {
                return new ApiClient(baseUrl, clientInfo);
            });
        }
    }
}
