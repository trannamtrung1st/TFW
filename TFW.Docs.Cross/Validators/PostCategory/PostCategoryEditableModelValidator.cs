using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Docs.Cross.Validators.Setting;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.PostCategory
{
    public class PostCategoryEditableModelValidator : LocalizedSafeValidator<PostCategoryEditableModel, PostCategoryEditableModelValidator>
    {
        public PostCategoryEditableModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<PostCategoryEditableModelValidator> localizer) : base(validationResultProvider, localizer)
        {
        }
    }
}
