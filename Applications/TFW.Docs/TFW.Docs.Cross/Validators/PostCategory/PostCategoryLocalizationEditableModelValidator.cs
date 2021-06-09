using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.PostCategory;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Validations.Fluent.Extensions;

namespace TFW.Docs.Cross.Validators.PostCategory
{
    public class PostCategoryLocalizationEditableModelValidator
        : LocalizedSafeValidator<PostCategoryLocalizationEditableModel, PostCategoryLocalizationEditableModelValidator>
    {
        public PostCategoryLocalizationEditableModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<PostCategoryLocalizationEditableModelValidator> localizer,
            AppEntitySchema entitySchema) : base(validationResultProvider, localizer)
        {
            var pcLocalizationType = typeof(PostCategoryLocalizationEntity);

            RuleFor(model => model.Title).NotEmpty()
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest)
                .FollowSchema(entitySchema, pcLocalizationType, nameof(PostCategoryLocalizationEntity.Title))
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest);

            RuleFor(model => model.Description)
                .FollowSchema(entitySchema, pcLocalizationType, nameof(PostCategoryLocalizationEntity.Description))
                .WithState(model => ResultCode.PostCategory_InvalidCreatePostCategoryRequest);
        }
    }
}
