using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFW.Framework.Validations.Fluent.Helpers;
using TFW.Framework.Validations.Fluent.Providers;

namespace TFW.Framework.Validations.Fluent.Validators
{
    public abstract class SafeValidator<T> : AbstractValidator<T>
    {
        private readonly ISet<object> validatedObjects;
        protected readonly IValidationResultProvider validationResultProvider;

        protected SafeValidator()
        {
            validatedObjects = new HashSet<object>();
        }

        protected SafeValidator(IValidationResultProvider validationResultProvider)
        {
            this.validationResultProvider = validationResultProvider;
            validatedObjects = new HashSet<object>();
        }

        internal protected bool AddValidated(object obj)
        {
            return validatedObjects.Add(obj);
        }

        internal protected bool RemoveValidated(object obj)
        {
            return validatedObjects.Remove(obj);
        }

        internal protected bool IsValidated(object obj)
        {
            return validatedObjects.Contains(obj);
        }

        protected IConditionBuilder WhenInvokedByMvc(Action action, bool isValue = true)
        {
            return When((obj, context) => context.IsInvokedByMvc() == isValue, action);
        }

        protected void IncludeBaseValidators(IServiceProvider serviceProvider)
        {
            var type = typeof(T).BaseType;

            while (type != typeof(object))
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(type);
                var baseValidator = serviceProvider.GetService(validatorType);

                if (baseValidator != null)
                    Include(baseValidator as IValidator<T>);

                type = type.BaseType;
            }
        }

        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var result = base.Validate(context);

            AddOrSkipValidationResult(context, result);

            return result;
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context, CancellationToken cancellation = default)
        {
            var result = await base.ValidateAsync(context, cancellation);

            AddOrSkipValidationResult(context, result);

            return result;
        }

        private void AddOrSkipValidationResult(ValidationContext<T> context, ValidationResult result)
        {
            if (!context.IsChildContext)
                validationResultProvider?.Results.Add(result);
        }
    }
}
