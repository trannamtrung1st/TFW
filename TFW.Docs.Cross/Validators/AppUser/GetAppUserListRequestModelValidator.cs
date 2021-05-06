using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Docs.Cross.Models.Common;
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
                RuleForEach(request => request.GetFieldsArr()).Cascade(CascadeMode.Stop)
                    .Must(field => GetListAppUsersRequestModel.Projections.ContainsKey(field))
                    .WithName(BaseGetListRequestModel.Parameters.Fields)
                    .WithState(request => ResultCode.InvalidProjectionRequest);
            });

            When(request => request.GetSortByArr() != null, () =>
            {
                RuleForEach(request => request.GetSortByArr()).Cascade(CascadeMode.Stop)
                    .MinimumLength(2)
                    .Must(field => GetListAppUsersRequestModel.SortOptions.Contains(field.Substring(1)))
                    .WithName(BaseGetListRequestModel.Parameters.SortBy)
                    .WithState(request => ResultCode.InvalidSortingRequest);
            });
        }
    }
}
