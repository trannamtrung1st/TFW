using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Cross.Validators.AppUser
{
    public class GetAppUserListRequestModelValidator : SafeValidator<GetAppUserListRequestModel>
    {
        public GetAppUserListRequestModelValidator(IServiceProvider serviceProvider)
        {
            IncludeBaseValidators(serviceProvider);

            When(request => request.GetFieldsArr() != null, () =>
            {
                RuleForEach(request => request.GetFieldsArr())
                    .Must(field => DynamicQueryAppUserModel.Projections.ContainsKey(field))
                    .OverridePropertyName(nameof(GetAppUserListRequestModel.fields))
                    .WithState(request => ResultCode.InvalidProjectionRequest);
            });

            When(request => request.GetSortByArr() != null, () =>
            {
                RuleForEach(request => request.GetSortByArr())
                    .MinimumLength(2)
                    .Must(field => DynamicQueryAppUserModel.SortOptions.Contains(field.Substring(1)))
                    .OverridePropertyName(nameof(GetAppUserListRequestModel.sortBy))
                    .WithState(request => ResultCode.InvalidSortingRequest);
            });
        }
    }
}
