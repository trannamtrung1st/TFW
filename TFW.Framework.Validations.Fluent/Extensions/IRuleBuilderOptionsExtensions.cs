using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.Validations.Fluent.Extensions
{
    public static class IRuleBuilderOptionsExtensions
    {
        public static IRuleBuilderOptions<TIn, TOut> WhenNotValidated<TIn, TOut>(this IRuleBuilderOptions<TIn, TOut> builder,
            SafeValidator<TIn> validator, Func<TIn, object> objProvider,
            ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators)
        {
            return builder.When((obj) => validator.AddValidated(objProvider(obj)), applyConditionTo);
        }
    }
}
