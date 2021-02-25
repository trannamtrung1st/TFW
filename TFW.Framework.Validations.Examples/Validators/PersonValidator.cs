using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Validations.Examples.Models;

namespace TFW.Framework.Validations.Examples.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(person => person.FullName).NotEmpty();
        }
    }
}
