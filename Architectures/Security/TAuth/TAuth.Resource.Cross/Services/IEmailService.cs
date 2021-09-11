﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAuth.Resource.Cross.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string emailTo, string subject, string body);
    }

    public class MockEmailService : IEmailService
    {
        public Task SendEmailAsync(string emailTo, string subject, string body)
        {
            Console.WriteLine(emailTo);
            Console.WriteLine(subject);
            Console.WriteLine(body);

            return Task.CompletedTask;
        }
    }
}
