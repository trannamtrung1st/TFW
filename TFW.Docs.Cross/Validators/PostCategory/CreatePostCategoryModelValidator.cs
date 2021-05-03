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
    public class CreatePostCategoryModelValidator : LocalizedSafeValidator<CreatePostCategoryModel, CreatePostCategoryModelValidator>
    {
        public CreatePostCategoryModelValidator(IValidationResultProvider validationResultProvider,
            IServiceProvider serviceProvider,
            IStringLocalizer<CreatePostCategoryModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            IncludeBaseValidators(serviceProvider);

            RuleFor(model => model.ListOfLocalization)
                .NotEmpty()
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest)
                .Must(model =>
                {
                    return model.Where(o => o.IsDefault).Count() == 1
                        && !model.GroupBy(o => new
                        {
                            o.Lang,
                            o.Region
                        }).Any(group => group.Count() > 1);
                })
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest);
        }
    }
}
