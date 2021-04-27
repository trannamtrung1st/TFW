using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.Identity
{
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
        public static class Message
        {
            public const string ConfirmPasswordDoesNotMatch = nameof(ConfirmPasswordDoesNotMatch);
        }

        public RegisterModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<RegisterModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            RuleFor(model => model.username)
                .NotEmpty().InvalidState()
                .Length(5, 100).InvalidState();

            RuleFor(model => model.password)
                .NotEmpty().InvalidState()
                .MinimumLength(6).InvalidState()
                .MaximumLength(100).InvalidState();

            RuleFor(model => model.confirmPassword)
                .Equal(model => model.password).WithMessage(localizer[Message.ConfirmPasswordDoesNotMatch])
                .InvalidState();

            RuleFor(model => model.fullName)
                .MaximumLength(100).InvalidState();

            RuleFor(model => model.email)
                .EmailAddress().InvalidState()
                .MaximumLength(100).InvalidState();
        }
    }
}
