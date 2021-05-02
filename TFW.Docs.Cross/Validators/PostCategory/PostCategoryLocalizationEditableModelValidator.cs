using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.PostCategory
{
    public class PostCategoryLocalizationEditableModelValidator
        : LocalizedSafeValidator<PostCategoryLocalizationEditableModel, PostCategoryLocalizationEditableModelValidator>
    {
        public PostCategoryLocalizationEditableModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<PostCategoryLocalizationEditableModelValidator> localizer) : base(validationResultProvider, localizer)
        {
        }
    }
}
