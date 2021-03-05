using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Exceptions;
using TFW.Cross.Models.Identity;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Cross.Validators.Identity
{
    public class RequestTokenModelValidator : SafeValidator<RequestTokenModel>
    {
        public RequestTokenModelValidator(IValidationResultProvider validationResultProvider)
            : base(validationResultProvider)
        {
            CascadeMode = CascadeMode.Stop;

            var invalidRequest = OAuthException.InvalidRequest();

            RuleFor(request => request.grant_type).NotEmpty()
                .WithState(request => invalidRequest);

            When(request => request.grant_type == SecurityConsts.GrantType.Password, () =>
            {
                RuleFor(request => request.username).NotEmpty()
                    .WithState(request => invalidRequest);

                RuleFor(request => request.password).NotEmpty()
                    .WithState(request => invalidRequest);
            });

            When(request => request.grant_type == SecurityConsts.GrantType.RefreshToken, () =>
            {
                RuleFor(request => request.refresh_token).NotEmpty()
                    .WithState(request => invalidRequest);
            });
        }
    }
}
