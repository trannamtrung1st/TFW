using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Validations.Fluent.Interceptors
{
    public class ConsoleLoggingInterceptor : IValidatorInterceptor
    {
        public ConsoleLoggingInterceptor()
        {
        }

        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, IValidationContext commonContext, ValidationResult result)
        {
            Console.WriteLine("After MVC validation");

            return result;
        }

        public IValidationContext BeforeMvcValidation(ControllerContext controllerContext, IValidationContext commonContext)
        {
            Console.WriteLine("Before MVC validation");

            return commonContext;
        }
    }
}
