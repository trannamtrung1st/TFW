using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.Common
{
    public class BaseListRequestModelValidator : LocalizedSafeValidator<BaseListRequestModel, BaseListRequestModelValidator>
    {
        public BaseListRequestModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<BaseListRequestModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            RuleFor(request => request.Page)
                .GreaterThanOrEqualTo(0)
                .WithState(request => ResultCode.InvalidPagingRequest);

            RuleFor(request => request.PageLimit)
                .GreaterThan(0)
                .WithState(request => ResultCode.InvalidPagingRequest);
        }
    }
}
