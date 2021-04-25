using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Exceptions;
using TFW.Docs.Cross.Models.Identity;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.Identity
{
    public class RequestTokenModelValidator : LocalizedSafeValidator<RequestTokenModel, RequestTokenModelValidator>
    {
        public RequestTokenModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<RequestTokenModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            CascadeMode = CascadeMode.Stop;

            var invalidRequest = OAuthException.InvalidRequest();

            RuleFor(request => request.grant_type).NotEmpty()
                .WithState(request => invalidRequest);

            When(request => request.grant_type == SecurityConsts.GrantTypes.Password, () =>
            {
                RuleFor(request => request.username).NotEmpty()
                    .WithState(request => invalidRequest);

                RuleFor(request => request.password).NotEmpty()
                    .WithState(request => invalidRequest);
            });

            When(request => request.grant_type == SecurityConsts.GrantTypes.RefreshToken, () =>
            {
                RuleFor(request => request.refresh_token).NotEmpty()
                    .WithState(request => invalidRequest);
            });
        }
    }
}
