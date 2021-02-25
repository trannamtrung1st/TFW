using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Validations.Examples.Models;

namespace TFW.Framework.Validations.Examples.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Surname).NotNull();

        }
    }
}
