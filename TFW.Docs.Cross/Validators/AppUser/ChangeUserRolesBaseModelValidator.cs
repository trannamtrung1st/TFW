using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;
using TFW.Docs.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent;

namespace TFW.Docs.Cross.Validators.AppUser
{
    public class ChangeUserRolesBaseModelValidator : LocalizedSafeValidator<ChangeUserRolesBaseModel, ChangeUserRolesBaseModelValidator>
    {
        public static class Message
        {
            public const string InvalidRoleName = nameof(InvalidRoleName);
        }

        public ChangeUserRolesBaseModelValidator(IValidationResultProvider validationResultProvider,
            IStringLocalizer<ChangeUserRolesBaseModelValidator> localizer) : base(validationResultProvider, localizer)
        {
            RuleFor(model => model.Username).NotEmpty()
                .WithState(model => ResultCode.Identity_InvalidChangeUserRolesRequest);

            RuleFor(model => model.Roles).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(roles => roles.All(role => RoleName.All.Contains(role)))
                .WithMessage(localizer[Message.InvalidRoleName])
                .WithState(model => ResultCode.Identity_InvalidChangeUserRolesRequest);
        }
    }
}
