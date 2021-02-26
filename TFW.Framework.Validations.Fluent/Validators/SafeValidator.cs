using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Validations.Fluent.Helpers;

namespace TFW.Framework.Validations.Fluent.Validators
{
    public abstract class SafeValidator<T> : AbstractValidator<T>
    {
        private readonly ISet<object> validatedObjects;

        protected SafeValidator()
        {
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
    }
}
