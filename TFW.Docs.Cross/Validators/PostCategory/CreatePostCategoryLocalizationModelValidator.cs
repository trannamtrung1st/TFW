using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.PostCategory
{
    public class CreatePostCategoryLocalizationModelValidator
        : LocalizedSafeValidator<CreatePostCategoryLocalizationModel, CreatePostCategoryLocalizationModelValidator>
    {
        public CreatePostCategoryLocalizationModelValidator(IValidationResultProvider validationResultProvider,
            IServiceProvider serviceProvider,
            IStringLocalizer<CreatePostCategoryLocalizationModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            IncludeBaseValidators(serviceProvider);

            RuleFor(model => model.Lang).NotEmpty()
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest)
                .Length(2)
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest);

            RuleFor(model => model.Region).NotNull()
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest)
                .Length(2)
                .When(model => !string.IsNullOrEmpty(model.Region), ApplyConditionTo.CurrentValidator)
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest);
        }
    }
}
