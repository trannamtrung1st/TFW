using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Cross.Schema;

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

        public static IRuleBuilderOptions<TIn, string> FollowSchema<TIn>(
            this IRuleBuilder<TIn, string> builder,
            EntitySchema schema, Type entityType, string propName)
        {
            var prop = schema.GetString(entityType, propName);

            if (prop == null) throw new KeyNotFoundException();

            if (prop.IsUnboundLength) return builder.MaximumLength(int.MaxValue);

            if (prop.IsFixLength) return builder.Length(prop.StringLength.Value);

            return builder.MaximumLength(prop.StringLength.Value);
        }
    }
}
