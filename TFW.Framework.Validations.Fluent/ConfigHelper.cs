using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TFW.Framework.Validations.Fluent.Common;
using TFW.Framework.Validations.Fluent.Providers;

namespace TFW.Framework.Validations.Fluent
{
    // ASP.NET Core: https://docs.fluentvalidation.net/en/latest/aspnet.html
    public static class ConfigHelper
    {
        public static IServiceCollection AddDefaultValidationResultProvider(this IServiceCollection services)
        {
            return services.AddScoped<IValidationResultProvider, DefaultValidationResultProvider>();
        }

        public static IMvcBuilder AddDefaultFluentValidation(this IMvcBuilder mvcBuilder, IEnumerable<Assembly> assemblies,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            return mvcBuilder.AddFluentValidation(opt =>
            {
                ConfigureFluentValidationMvc(opt, assemblies, serviceLifetime);
            });
        }

        public static IMvcCoreBuilder AddDefaultFluentValidation(this IMvcCoreBuilder mvcBuilder, IEnumerable<Assembly> assemblies,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            return mvcBuilder.AddFluentValidation(opt =>
            {
                ConfigureFluentValidationMvc(opt, assemblies, serviceLifetime);
            });
        }

        private static void ConfigureFluentValidationMvc(FluentValidationMvcConfiguration opt, IEnumerable<Assembly> assemblies,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            opt.RegisterValidatorsFromAssemblies(assemblies, lifetime: serviceLifetime);
            opt.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            opt.ImplicitlyValidateChildProperties = true;
            opt.ImplicitlyValidateRootCollectionElements = false;
            opt.AutomaticValidationEnabled = true;
            opt.ValidatorOptions.DisplayNameResolver = DisplayNameResolver.Resolve;
        }
    }
}
