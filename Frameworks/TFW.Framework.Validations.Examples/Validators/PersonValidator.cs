using FluentValidation;
using TFW.Framework.Validations.Examples.Models;

namespace TFW.Framework.Validations.Examples.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleSet("RealName", () =>
            {
                RuleFor(person => person.FullName).MinimumLength(6);
            });

            RuleFor(person => person.FullName).NotEmpty();
        }
    }
}
