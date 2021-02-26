using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Validations.ModelValidation.Validators;

namespace TFW.Framework.Validations.ModelValidation
{
    public static class ConfigHelper
    {
        public static IServiceCollection DisableModelValidation(this IServiceCollection services)
        {
            return services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
        }

    }
}
