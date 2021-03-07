using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Cross.Validators.Identity
{
    public class RegisterModelValidator : SafeValidator<RegisterModel>
    {
        public RegisterModelValidator(IValidationResultProvider validationResultProvider) : base(validationResultProvider)
        {
            RuleFor(model => model.username)
                .NotEmpty().MinimumLength(5).MaximumLength(100)
                .WithState(model => ResultCode.Identity_InvalidRegisterRequest);

            RuleFor(model => model.password)
                .NotEmpty().MinimumLength(6).MaximumLength(100)
                .WithState(model => ResultCode.Identity_InvalidRegisterRequest);

            RuleFor(model => model.confirmPassword)
                .Equal(model => model.password).WithMessage("Confirmation password does not match")
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
