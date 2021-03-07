using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using TFW.Framework.SimpleMail.Helpers;
using TFW.Framework.SimpleMail.Models;
using TFW.Framework.SimpleMail.Options;
using TFW.Framework.SimpleMail.Services;

namespace TFW.Framework.Mail.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddOptions()
                .Configure<SmtpOption>(opt =>
                {
                    opt.Host = "smtp.gmail.com";
                    opt.Port = 465;
                    opt.UseSsl = true;
                    opt.UserName = "...";
                    opt.Password = "...";
                    opt.QuitAfterSending = true;
                });

            var provider = services.BuildServiceProvider();
            
            var optionSnapshot = provider.GetRequiredService<IOptionsSnapshot<SmtpOption>>();

            var message1 = new MimeMessage()
                .AddFrom("Trung Tran", "...")
                .AddTo("Another Trung Tran", "...")
                .Subject("You will be OK?")
                .Body(html: "<div style='color:green;font-weight:bold'>It's OK</div>",
                    SimpleAttachment.From(@"\somefile1"),
                    SimpleAttachment.From(@"\somefile2"));

            var message2 = new MimeMessage()
                .AddFrom("Trung Tran", "...")
                .AddTo("Another Trung Tran", "...")
                .Subject("You will be OK? (twice)")
                .Body(html: "<div style='color:green;font-weight:bold'>It's OK</div>");

            ISmtpService smtpService = new SmtpService(optionSnapshot);

            smtpService.SendMailAsync(message1, message2).Wait();
        }
    }
}
