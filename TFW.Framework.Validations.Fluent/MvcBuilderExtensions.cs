using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace TFW.Framework.Validations.Fluent
{
    public static class MvcBuilderExtensions
    {
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
