using FluentValidation;
using System.Linq;
using TFW.Cross.Models.AppUser;
using TFW.Framework.Validations.Fluent.Providers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Cross.Validators.AppUser
{
    public class ChangeUserRolesBaseModelValidator : SafeValidator<ChangeUserRolesBaseModel>
    {
        public ChangeUserRolesBaseModelValidator(IValidationResultProvider validationResultProvider) : base(validationResultProvider)
        {
            RuleFor(model => model.Username).NotEmpty()
                .WithState(model => ResultCode.Identity_InvalidChangeUserRolesRequest);

            RuleFor(model => model.Roles).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(roles => roles.All(role => RoleName.All.Contains(role)))
                .WithMessage("Invalid role name")
                .WithState(model => ResultCode.Identity_InvalidChangeUserRolesRequest);
        }
    }
}
