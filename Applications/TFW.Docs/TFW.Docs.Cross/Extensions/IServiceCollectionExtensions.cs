using Microsoft.Extensions.DependencyInjection;
using TFW.Docs.Cross.Providers;

namespace TFW.Docs.Cross.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddNullBusinessContextProvider(this IServiceCollection services)
        {
            return services.AddSingleton<IBusinessContextProvider, NullBusinessContextProvider>();
        }
    }
}
