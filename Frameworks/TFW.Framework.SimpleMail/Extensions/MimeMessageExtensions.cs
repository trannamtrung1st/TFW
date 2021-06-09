using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace TFW.Framework.SimpleMail.Extensions
{
    public static class MimeMessageExtensions
    {
        #region Add mailbox address
        public static MimeMessage AddFrom(this MimeMessage message, params MailboxAddress[] addresses)
        {
            message.From.AddRange(addresses);

            return message;
        }

        public static MimeMessage AddTo(this MimeMessage message, params MailboxAddress[] addresses)
        {
            message.To.AddRange(addresses);

            return message;
        }

        public static MimeMessage AddCc(this MimeMessage message, params MailboxAddress[] addresses)
        {
            message.Cc.AddRange(addresses);

            return message;
        }

        public static MimeMessage AddBcc(this MimeMessage message, params MailboxAddress[] addresses)
        {
            message.Bcc.AddRange(addresses);

            return message;
        }

        public static MimeMessage AddReplyTo(this MimeMessage message, params MailboxAddress[] addresses)
        {
            message.ReplyTo.AddRange(addresses);

            return message;
        }

        public static MimeMessage AddFrom(this MimeMessage message, string name, string address)
        {
            message.From.Add(new MailboxAddress(name, address));

            return message;
        }

        public static MimeMessage AddTo(this MimeMessage message, string name, string address)
        {
            message.To.Add(new MailboxAddress(name, address));

            return message;
        }

        public static MimeMessage AddCc(this MimeMessage message, string name, string address)
        {
            message.Cc.Add(new MailboxAddress(name, address));

            return message;
        }

        public static MimeMessage AddBcc(this MimeMessage message, string name, string address)
        {
            message.Bcc.Add(new MailboxAddress(name, address));

            return message;
        }

        public static MimeMessage AddReplyTo(this MimeMessage message, string name, string address)
        {
            message.ReplyTo.Add(new MailboxAddress(name, address));

            return message;
        }
        #endregion

        public static MimeMessage Body(this MimeMessage message, string html, params SimpleAttachment[] attachments)
        {
            var bodyBuilder = new BodyBuilder();

            foreach (var att in attachments)
                if (att.ContentType != null)
                    bodyBuilder.Attachments.Add(att.FileName, att.DataStream, att.ContentType);
                else
                    bodyBuilder.Attachments.Add(att.FileName, att.DataStream);

            bodyBuilder.HtmlBody = html;

            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }

        public static MimeMessage Subject(this MimeMessage message, string subject)
        {
            message.Subject = subject;

            return message;
        }

        public static MimeMessage InReplyTo(this MimeMessage message, string messageId)
        {
            message.InReplyTo = messageId;

            return message;
        }
    }
}
