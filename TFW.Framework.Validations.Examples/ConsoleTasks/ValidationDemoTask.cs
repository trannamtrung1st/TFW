using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.ConsoleApp;
using TFW.Framework.Validations.Examples.Models;
using TFW.Framework.Validations.Examples.Validators;

namespace TFW.Framework.Validations.Examples.ConsoleTasks
{
    /// <summary>
    /// Resources:
    /// 1. Custom: https://docs.fluentvalidation.net/en/latest/custom-validators.html
    /// 2. Localization: https://docs.fluentvalidation.net/en/latest/localization.html
    /// 3. Test: https://docs.fluentvalidation.net/en/latest/testing.html
    /// </summary>

    public class ValidationDemoTask : DefaultConsoleTask
    {
        public override IDictionary<string, Func<Task>> Tasks => new Dictionary<string, Func<Task>>
        {
            { SimpleValidationOpt, SimpleValidation }
        };

        public override string Title => "Validation demo";

        public override string Description => $"Some basic demo for FluentValidation\n" +
            $"{SimpleValidationOpt}. Simple validation\n" +
            $"Input: ";

        private Task SimpleValidation()
        {
            Console.Clear();

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo(XConsole.PromptLine("Culture: "));

            Customer customer = new Customer();

            int customerId = 0;
            int.TryParse(XConsole.PromptLine("Id: "), out customerId);
            customer.Id = customerId;

            customer.Surname = XConsole.PromptLine("Surname: ");
            customer.Forename = XConsole.PromptLine("Forename: ");
            customer.FullName = customer.Surname + " " + customer.Forename;

            customer.Address = new Address();
            customer.Address.AddressLines.Add("");
            customer.Address.Country = XConsole.PromptLine("Country: ");
            customer.Address.ARandomGuy = new Customer
            {
                Address = new Address
                {
                    ARandomGuy = new Customer
                    {
                        Address = customer.Address
                    }
                }
            };

            ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) => {
                if (member != null)
                {
                    return member.Name + "Foo";
                }
                return null;
            };

            CustomerValidator validator = new CustomerValidator();

            ValidationResult result = validator.Validate(customer, 
                opt => opt.IncludeAllRuleSets());

            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
                }
            }

            Console.WriteLine("---------------");

            string allMessages = result.ToString("\n");
            Console.WriteLine(allMessages);

            try
            {
                Console.WriteLine("---------------");
            
                validator.Validate(customer, options => {
                    options.ThrowOnFailures();
                    options.IncludeRuleSets("MyRuleSets");
                    options.IncludeProperties(x => x.Surname);
                });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }

            Console.ReadLine();
            Console.Clear();

            return Task.CompletedTask;
        }

        public override async Task Start()
        {
            Console.Clear();

            var opt = XConsole.PromptLine(Description);

            switch (opt)
            {
                case SimpleValidationOpt:
                    await SimpleValidation();
                    break;
            }

            XConsole.PromptLine("\nPress enter to exit task");

            Console.Clear();
        }

        public const string SimpleValidationOpt = "1";
    }
}
