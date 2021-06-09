using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.Validations.Examples.Models;
using TFW.Framework.Validations.Fluent;
using TFW.Framework.Validations.Fluent.Extensions;

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

            RuleForEach(address => address.AddressLines).NotEmpty()
                .WhenAsync((address, token) => Task.FromResult(address.AddressLines.Count > 1));

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
