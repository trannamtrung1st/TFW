using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.PostCategory
{
    public class CreatePostCategoryModelValidator
        : CreateLocalizedModelValidator<CreatePostCategoryModel, CreatePostCategoryLocalizationModel, CreatePostCategoryModelValidator>
    {
        public CreatePostCategoryModelValidator(IValidationResultProvider validationResultProvider,
            IServiceProvider serviceProvider,
            IStringLocalizer<CreatePostCategoryModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            IncludeBaseValidators(serviceProvider);
        }

        protected override ResultCode DefaultEmptyListCode => ResultCode.PostCategory_InvalidCreatePostCategoryRequest;
    }
}
