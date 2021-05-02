using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.PostCategory
{
    public class CreatePostCategoryModelValidator : LocalizedSafeValidator<CreatePostCategoryModel, CreatePostCategoryModelValidator>
    {
        public CreatePostCategoryModelValidator(IValidationResultProvider validationResultProvider,
            IServiceProvider serviceProvider,
            IStringLocalizer<CreatePostCategoryModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            IncludeBaseValidators(serviceProvider);

            RuleFor(model => model.ListOfLocalization)
                .NotEmpty()
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest);

            RuleForEach(model => model.ListOfLocalization)
                .InjectValidator();
        }
    }
}
