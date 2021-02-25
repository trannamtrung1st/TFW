using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Framework.Validations.Examples.Models;
using TFW.Framework.Validations.Fluent.Helpers;
using TFW.Framework.Validations.Fluent.Validators;

namespace TFW.Framework.Validations.Examples.Validators
{
    public class AddressValidator : SafeValidator<Address>
    {
        private CustomerValidator _customerValidator;

        public AddressValidator(CustomerValidator customerValidator)
        {
            _customerValidator = customerValidator;
         
            RuleFor(address => address.Country)
                .Cascade(CascadeMode.Continue).NotEmpty();

            RuleFor(address => address.AddressLines).NotEmpty();

            RuleForEach(address => address.AddressLines).NotEmpty();

            RuleFor(address => address.ARandomGuy)
                .NotNull()
                .SetValidator(new PersonValidator())
                .SetInheritanceValidator(opt =>
                {
                    opt.Add<Customer>(_customerValidator);
                })
                .WhenNotValidated(this, address => address.ARandomGuy);
        }
    }
}
