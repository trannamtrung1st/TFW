﻿using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.SimpleMail.Options
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
