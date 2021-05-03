using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators
{
    public abstract class LocalizationModelValidator<T, Context> : LocalizedSafeValidator<T, Context>
        where T : ILocalizationModel
    {
        public LocalizationModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<Context> localizer) : base(validationResultProvider, localizer)
        {
            var lang = RuleFor(model => model.Lang).NotEmpty();
            lang = HandleEmptyLang(lang).Length(2);
            lang = HandleInvalidLangLength(lang);

            var region = RuleFor(model => model.Region).NotNull();
            region = HandleNullRegion(region).Length(2)
                .When(model => !string.IsNullOrEmpty(model.Region), ApplyConditionTo.CurrentValidator);
            region = HandleInvalidRegionLength(region);
        }

        protected abstract ResultCode DefaultInvalidLangCode { get; }
        protected virtual ResultCode DefaultInvalidRegionCode => DefaultInvalidLangCode;

        protected virtual IRuleBuilderOptions<T, string> HandleEmptyLang(IRuleBuilderOptions<T, string> builder)
        {
            return builder.WithState(model => DefaultInvalidLangCode);
        }

        protected virtual IRuleBuilderOptions<T, string> HandleInvalidLangLength(IRuleBuilderOptions<T, string> builder)
        {
            return builder.WithState(model => DefaultInvalidLangCode);
        }
        protected virtual IRuleBuilderOptions<T, string> HandleNullRegion(IRuleBuilderOptions<T, string> builder)
        {
            return builder.WithState(model => DefaultInvalidRegionCode);
        }

        protected virtual IRuleBuilderOptions<T, string> HandleInvalidRegionLength(IRuleBuilderOptions<T, string> builder)
        {
            return builder.WithState(model => DefaultInvalidRegionCode);
        }
    }
}
