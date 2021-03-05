using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Common;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Cross.Validators.Common
{
    public class BaseGetListRequestModelValidator : SafeValidator<BaseGetListRequestModel>
    {
        public BaseGetListRequestModelValidator(IValidationResultProvider validationResultProvider)
            : base(validationResultProvider)
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
