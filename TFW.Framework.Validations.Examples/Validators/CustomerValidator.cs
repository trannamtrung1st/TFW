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
            Include(new PersonValidator());

            RuleFor(customer => customer.Surname).NotEmpty()
                .NotEqual("TNT");

            RuleFor(customer => customer.Id).GreaterThan(0)
                .WithErrorCode("CustomCode").WithMessage("My message: {PropertyName} (value: {PropertyValue}) is failed");

            RuleFor(customer => customer.Address).NotNull()
                // default error code
                .WithErrorCode("NotNullValidator").WithMessage((customer, address) => $"Hello address null")
                .DependentRules(()=>
                {
                    RuleFor(customer => customer.Address.ARandomGuy)
                        .Must(o => true)
                        .WithSeverity(Severity.Warning)
                        .WithState(o => 1001); // ResultCode enum ...
                });

            RuleFor(customer => customer.Address)
                .SetValidator(new AddressValidator(this))
                .WhenNotValidated(this, customer => customer.Address);

            When(o => o.Address != null, () =>
            {
                Console.WriteLine("Do some rule here");
            }).Otherwise(() =>
            {
                Console.WriteLine("Do some rule here (otherwise)");
            });
        }
    }
}
