using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Models;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators
{
    public abstract class CreateLocalizedModelValidator<T, LModel, Context> : LocalizedSafeValidator<T, Context>
        where T : ICreateLocalizedModel<LModel>
        where LModel : ILocalizationModel
    {
        public CreateLocalizedModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<Context> localizer) : base(validationResultProvider, localizer)
        {
            var list = RuleFor(model => model.ListOfLocalization).NotEmpty();
            list = HandleEmptyList(list)
                .Must(model =>
                {
                    return !model.GroupBy(o => new
                    {
                        o.Lang,
                        o.Region
                    }).Any(group => group.Count() > 1);
                });
            list = HandleInvalidList(list);
        }

        protected abstract ResultCode DefaultEmptyListCode { get; }
        protected virtual ResultCode DefaultInvalidListCode => DefaultEmptyListCode;

        protected virtual IRuleBuilderOptions<T, IEnumerable<LModel>> HandleEmptyList(
            IRuleBuilderOptions<T, IEnumerable<LModel>> builder)
        {
            return builder.WithState(model => DefaultEmptyListCode);
        }

        protected virtual IRuleBuilderOptions<T, IEnumerable<LModel>> HandleInvalidList(
            IRuleBuilderOptions<T, IEnumerable<LModel>> builder)
        {
            return builder.WithState(model => DefaultInvalidListCode);
        }
    }
}
