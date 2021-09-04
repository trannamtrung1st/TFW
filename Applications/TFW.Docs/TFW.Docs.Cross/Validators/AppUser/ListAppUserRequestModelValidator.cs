using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Docs.Cross.Models.Common;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.AppUser
{
    public class ListAppUserRequestModelValidator : LocalizedSafeValidator<ListAppUserRequestModel, ListAppUserRequestModelValidator>
    {
        public ListAppUserRequestModelValidator(IValidationResultProvider validationResultProvider,
            IServiceProvider serviceProvider,
            IStringLocalizer<ListAppUserRequestModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            IncludeBaseValidators(serviceProvider);

            When(request => request.GetFieldsArr() != null, () =>
            {
                RuleForEach(request => request.GetFieldsArr()).Cascade(CascadeMode.Stop)
                    .Must(field => ListAppUserRequestModel.Projections.ContainsKey(field))
                    .WithName(BaseListRequestModel.Parameters.Fields)
                    .WithState(request => ResultCode.InvalidProjectionRequest);
            });

            When(request => request.GetSortByArr() != null, () =>
            {
                RuleForEach(request => request.GetSortByArr()).Cascade(CascadeMode.Stop)
                    .MinimumLength(2)
                    .Must(field => ListAppUserRequestModel.SortOptions.Contains(field.Substring(1)))
                    .WithName(BaseListRequestModel.Parameters.SortBy)
                    .WithState(request => ResultCode.InvalidSortingRequest);
            });
        }
    }
}
