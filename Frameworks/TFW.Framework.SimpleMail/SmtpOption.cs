using MailKit.Security;

namespace TFW.Framework.SimpleMail
{
    public class SmtpOption
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public SecureSocketOptions? SecureSocketOptions { get; set; }
        public bool QuitAfterSending { get; set; } = true;

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
