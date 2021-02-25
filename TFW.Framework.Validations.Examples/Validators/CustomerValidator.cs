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
            RuleFor(customer => customer.Surname).NotEmpty()
                .NotEqual("TNT");
            
            RuleFor(customer => customer.Id).GreaterThan(0);

            RuleFor(customer => customer.Address).NotNull();

            RuleFor(customer => customer.Address).SetValidator(new AddressValidator())
                .When(customer => customer.Address != null);
        }
    }
}
