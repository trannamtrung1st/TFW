using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Threading.Tasks;

namespace TFW.Framework.SimpleMail
{
    public interface ISmtpService
    {
        Task SendMailAsync(params MimeMessage[] messages);
        Task SendMailAsync(string optionsName, params MimeMessage[] messages);
        Task SendMailAsync(SmtpOption option, params MimeMessage[] messages);
    }

    public class SmtpService : ISmtpService
    {
        protected readonly IOptionsSnapshot<SmtpOption> optionSnapshot;

        public SmtpService(IOptionsSnapshot<SmtpOption> optionSnapshot)
        {
            this.optionSnapshot = optionSnapshot;
        }

        public Task SendMailAsync(params MimeMessage[] messages)
        {
            return SendMailAsync(optionSnapshot.Value, messages);
        }

        public Task SendMailAsync(string optionsName, params MimeMessage[] messages)
        {
            var option = optionSnapshot.Get(optionsName);

            return SendMailAsync(option, messages);
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
