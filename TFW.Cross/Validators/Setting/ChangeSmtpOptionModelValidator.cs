using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Setting;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Cross.Validators.Setting
{
    public class ChangeSmtpOptionModelValidator : SafeValidator<ChangeSmtpOptionModel>
    {
        public ChangeSmtpOptionModelValidator(IValidationResultProvider validationResultProvider) : base(validationResultProvider)
        {
            RuleFor(model => model.UserName).NotEmpty()
                .WithState(model => ResultCode.Setting_InvalidChangeSmtpOptionRequest);

            RuleFor(model => model.Password).NotEmpty()
                .WithState(model => ResultCode.Setting_InvalidChangeSmtpOptionRequest);
        }
    }
}
