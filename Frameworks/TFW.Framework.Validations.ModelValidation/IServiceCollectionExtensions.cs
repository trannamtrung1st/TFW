using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace TFW.Framework.Validations.ModelValidation
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection DisableModelValidation(this IServiceCollection services)
        {
            return services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
        }

    }
}
