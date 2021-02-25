using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Validations.Examples.Models;
using TFW.Framework.Validations.Fluent.Helpers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Framework.Validations.Examples.Validators
{
    // scoped service
    public class CustomerValidator : SafeValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(customer => customer.Surname).NotEmpty()
                .NotEqual("TNT");

            RuleFor(customer => customer.Id).GreaterThan(0).WithMessage("My message: {PropertyName} is failed");

            RuleFor(customer => customer.Address).NotNull();

            RuleFor(customer => customer.Address)
                .SetValidator(new AddressValidator(this))
                .WhenNotValidated(this, customer => customer.Address);
        }
    }
}
