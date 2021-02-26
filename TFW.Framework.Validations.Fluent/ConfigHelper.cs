using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TFW.Framework.Validations.Fluent
{
    // ASP.NET Core: https://docs.fluentvalidation.net/en/latest/aspnet.html
    public static class ConfigHelper
    {
        public static IMvcBuilder AddDefaultFluentValidation(this IMvcBuilder mvcBuilder, IEnumerable<Assembly> assemblies,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            return mvcBuilder.AddFluentValidation(opt =>
            {
                Configure(opt, assemblies, serviceLifetime);
            });
        }

        public static IMvcCoreBuilder AddDefaultFluentValidation(this IMvcCoreBuilder mvcBuilder, IEnumerable<Assembly> assemblies,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            return mvcBuilder.AddFluentValidation(opt =>
            {
                Configure(opt, assemblies, serviceLifetime);
            });
        }

        private static void Configure(FluentValidationMvcConfiguration opt, IEnumerable<Assembly> assemblies,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            opt.RegisterValidatorsFromAssemblies(assemblies, lifetime: serviceLifetime);
            opt.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
            opt.ImplicitlyValidateChildProperties = true;
            opt.ImplicitlyValidateRootCollectionElements = false;
            opt.AutomaticValidationEnabled = true;
        }
    }
}
