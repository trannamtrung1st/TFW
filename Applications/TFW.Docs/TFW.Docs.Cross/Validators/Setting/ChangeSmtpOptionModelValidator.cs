using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.Setting;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.Setting
{
    public class ChangeSmtpOptionModelValidator : LocalizedSafeValidator<ChangeSmtpOptionModel, ChangeSmtpOptionModelValidator>
    {
        public ChangeSmtpOptionModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<ChangeSmtpOptionModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            RuleFor(model => model.UserName).NotEmpty()
                .WithState(model => ResultCode.Setting_InvalidChangeSmtpOptionRequest)
                .Length(ChangeSmtpOptionModel.UserNameLength[0], ChangeSmtpOptionModel.UserNameLength[1])
                .WithState(model => ResultCode.Setting_InvalidChangeSmtpOptionRequest);

            RuleFor(model => model.Password).NotEmpty()
                .WithState(model => ResultCode.Setting_InvalidChangeSmtpOptionRequest)
                .Length(ChangeSmtpOptionModel.PasswordLength[0], ChangeSmtpOptionModel.PasswordLength[1])
                .WithState(model => ResultCode.Setting_InvalidChangeSmtpOptionRequest);
        }
    }
}
