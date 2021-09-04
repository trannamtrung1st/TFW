using FluentValidation;
using Microsoft.Extensions.Localization;
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

            RuleFor(request => request.GrantType).NotEmpty()
                .WithState(request => invalidRequest);

            When(request => request.GrantType == SecurityConsts.GrantTypes.Password, () =>
            {
                RuleFor(request => request.Username).NotEmpty()
                    .WithState(request => invalidRequest);

                RuleFor(request => request.Password).NotEmpty()
                    .WithState(request => invalidRequest);
            });

            When(request => request.GrantType == SecurityConsts.GrantTypes.RefreshToken, () =>
            {
                RuleFor(request => request.RefreshToken).NotEmpty()
                    .WithState(request => invalidRequest);
            });
        }
    }
}
