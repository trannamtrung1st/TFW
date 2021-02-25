﻿using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.ConsoleApp;
using TFW.Framework.Validations.Examples.Models;
using TFW.Framework.Validations.Examples.Validators;

namespace TFW.Framework.Validations.Examples.ConsoleTasks
{
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

            Customer customer = new Customer();

            int customerId = 0;
            int.TryParse(XConsole.PromptLine("Id: "), out customerId);
            customer.Id = customerId;

            customer.Surname = XConsole.PromptLine("Surname: ");
            customer.Forename = XConsole.PromptLine("Forename: ");

            customer.Address = new Address();
            customer.Address.Country = XConsole.PromptLine("Country: ");

            CustomerValidator validator = new CustomerValidator();

            ValidationResult result = validator.Validate(customer);

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
