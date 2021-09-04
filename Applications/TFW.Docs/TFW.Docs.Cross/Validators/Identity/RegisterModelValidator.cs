using FluentValidation;
using Microsoft.Extensions.Localization;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Validations.Fluent.Extensions;

namespace TFW.Docs.Cross.Validators.Identity
{
    using Resources = AppResources.Validators.Identity.RegisterModelValidator;

    internal static class RegisterModelRuleBuilderExtensions
    {
        public static IRuleBuilderOptions<RegisterModel, T> InvalidState<T>(this
            IRuleBuilderOptions<RegisterModel, T> builder)
        {
            return builder.WithState(model => ResultCode.Identity_InvalidRegisterRequest);
        }
    }

    public class RegisterModelValidator : LocalizedSafeValidator<RegisterModel, RegisterModelValidator>
    {
        public RegisterModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<RegisterModelValidator> localizer,
            AppEntitySchema entitySchema) : base(validationResultProvider, localizer)
        {
            var appUserType = typeof(AppUserEntity);

            RuleFor(model => model.Username)
                .NotEmpty().InvalidState()
                .MinimumLength(SecurityConsts.AccountConstraints.UsernameMinLength).InvalidState()
                .FollowSchema(entitySchema, appUserType, nameof(AppUserEntity.UserName)).InvalidState();

            RuleFor(model => model.Password)
                .NotEmpty().InvalidState()
                .Length(SecurityConsts.AccountConstraints.PasswordMinLength,
                    SecurityConsts.AccountConstraints.PasswordMaxLength).InvalidState();

            RuleFor(model => model.ConfirmPassword)
                .Equal(model => model.Password).WithMessage(localizer[Resources.ConfirmPasswordDoesNotMatch])
                .InvalidState();

            RuleFor(model => model.FullName)
                .FollowSchema(entitySchema, appUserType, nameof(AppUserEntity.FullName)).InvalidState();

            RuleFor(model => model.Email)
                .EmailAddress().InvalidState()
                .FollowSchema(entitySchema, appUserType, nameof(AppUserEntity.Email)).InvalidState();
        }
    }
}
