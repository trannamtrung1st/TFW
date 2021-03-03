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
            var options = Options.Create<SmtpOption>(new SmtpOption
            {
                Host = "smtp.gmail.com",
                Port = 465,
                UseSsl = true,
                UserName = "...",
                Password = "...",
                QuitAfterSending = true
            });

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

            ISmtpService smtpService = new SmtpService(options);
            smtpService.SendMailAsync(message1, message2).Wait();
        }
    }
}
