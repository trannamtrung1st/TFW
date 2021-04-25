using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.AppUser
{
    public class GetAppUserListRequestModelValidator : LocalizedSafeValidator<GetListAppUsersRequestModel, GetAppUserListRequestModelValidator>
    {
        public GetAppUserListRequestModelValidator(IValidationResultProvider validationResultProvider,
            IServiceProvider serviceProvider, 
            IStringLocalizer<GetAppUserListRequestModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            IncludeBaseValidators(serviceProvider);

            When(request => request.GetFieldsArr() != null, () =>
            {
                RuleForEach(request => request.GetFieldsArr())
                    .Must(field => DynamicQueryAppUserModel.Projections.ContainsKey(field))
                    .WithName(nameof(GetListAppUsersRequestModel.fields))
                    .WithState(request => ResultCode.InvalidProjectionRequest);
            });

            When(request => request.GetSortByArr() != null, () =>
            {
                RuleForEach(request => request.GetSortByArr())
                    .MinimumLength(2)
                    .Must(field => DynamicQueryAppUserModel.SortOptions.Contains(field.Substring(1)))
                    .WithName(nameof(GetListAppUsersRequestModel.sortBy))
                    .WithState(request => ResultCode.InvalidSortingRequest);
            });
        }
    }
}
