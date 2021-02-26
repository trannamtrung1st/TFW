using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Framework.Validations.Fluent.Helpers
{
    public static class ValidatorHelper
    {
        public static IRuleBuilderOptions<TIn, TOut> WhenNotValidated<TIn, TOut>(this IRuleBuilderOptions<TIn, TOut> builder,
            SafeValidator<TIn> validator, Func<TIn, object> objProvider,
            ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
        {
            return builder.When((obj) => validator.AddValidated(objProvider(obj)), applyConditionTo);
        }

        public static bool IsInvokedByMvc<T>(this ValidationContext<T> context)
        {
            return context.RootContextData?.ContainsKey(RootContextDataKey.InvokedByMvc) == true
                && (bool)context.RootContextData[RootContextDataKey.InvokedByMvc] == true;
        }
    }
}
