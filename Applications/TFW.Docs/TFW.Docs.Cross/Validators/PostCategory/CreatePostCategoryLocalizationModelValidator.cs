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
        : LocalizationModelValidator<CreatePostCategoryLocalizationModel, CreatePostCategoryLocalizationModelValidator>
    {
        public CreatePostCategoryLocalizationModelValidator(IValidationResultProvider validationResultProvider,
            IServiceProvider serviceProvider,
            IStringLocalizer<CreatePostCategoryLocalizationModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            IncludeBaseValidators(serviceProvider);
        }

        protected override ResultCode DefaultInvalidLangCode => ResultCode.PostCategory_InvalidCreatePostCategoryLocalizationRequest;
    }
}
