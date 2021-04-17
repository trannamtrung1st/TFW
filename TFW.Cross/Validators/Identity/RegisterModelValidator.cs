using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent;

namespace TFW.Cross.Validators.Identity
{
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
                .NotEmpty().MinimumLength(5).MaximumLength(100)
                .WithState(model => ResultCode.Identity_InvalidRegisterRequest);

            RuleFor(model => model.password)
                .NotEmpty().MinimumLength(6).MaximumLength(100)
                .WithState(model => ResultCode.Identity_InvalidRegisterRequest);

            RuleFor(model => model.confirmPassword)
                .Equal(model => model.password).WithMessage(localizer[Message.ConfirmPasswordDoesNotMatch])
                .WithState(model => ResultCode.Identity_InvalidRegisterRequest);

            RuleFor(model => model.fullName)
                .NotEmpty().MaximumLength(100)
                .WithState(model => ResultCode.Identity_InvalidRegisterRequest);

            RuleFor(model => model.email)
                .EmailAddress().MaximumLength(100)
                .WithState(model => ResultCode.Identity_InvalidRegisterRequest);
        }
    }
}
