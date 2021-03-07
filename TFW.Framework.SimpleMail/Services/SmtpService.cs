using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TFW.Framework.SimpleMail.Options;

namespace TFW.Framework.SimpleMail.Services
{
    public interface ISmtpService
    {
        Task SendMailAsync(params MimeMessage[] messages);
        Task SendMailAsync(SmtpOption option, params MimeMessage[] messages);
    }

    public class SmtpService : ISmtpService
    {
        protected readonly SmtpOption defaultOption;

        public SmtpService(IOptionsSnapshot<SmtpOption> defaultOption)
        {
            this.defaultOption = defaultOption.Value;
        }

        public Task SendMailAsync(params MimeMessage[] messages)
        {
            return SendMailAsync(defaultOption, messages);
        }

        public async Task SendMailAsync(SmtpOption option, params MimeMessage[] messages)
        {
            using (var client = new SmtpClient())
            {
                if (option.SecureSocketOptions.HasValue)
                    client.Connect(option.Host, option.Port, option.SecureSocketOptions.Value);
                else
                    client.Connect(option.Host, option.Port, option.UseSsl);

                client.Authenticate(new NetworkCredential(option.UserName, option.Password));

                foreach (var message in messages)
                    await client.SendAsync(message);

                await client.DisconnectAsync(option.QuitAfterSending);
            }
        }
    }
}
