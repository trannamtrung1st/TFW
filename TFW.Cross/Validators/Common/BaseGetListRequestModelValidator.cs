using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Common;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Cross.Validators.Common
{
    public class BaseGetListRequestModelValidator : LocalizedSafeValidator<BaseGetListRequestModel, BaseGetListRequestModelValidator>
    {
        public BaseGetListRequestModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<BaseGetListRequestModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            RuleFor(request => request.page)
                .GreaterThanOrEqualTo(0)
                .WithState(request => ResultCode.InvalidPagingRequest);

            RuleFor(request => request.pageLimit)
                .GreaterThan(0)
                .WithState(request => ResultCode.InvalidPagingRequest);
        }
    }
}
