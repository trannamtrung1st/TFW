using Microsoft.Extensions.DependencyInjection;

namespace TFW.Framework.Validations.Fluent
{
    // ASP.NET Core: https://docs.fluentvalidation.net/en/latest/aspnet.html
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultValidationResultProvider(this IServiceCollection services)
        {
            return services.AddScoped<IValidationResultProvider, DefaultValidationResultProvider>();
        }
    }
}
