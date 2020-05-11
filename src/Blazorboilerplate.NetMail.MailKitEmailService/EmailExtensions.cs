using BlazorBoilerplate.Shared.Email;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BlazorBoilerplate.NetMail.MailKitEmailService
{
    public static class EmailExtensions
    {
        public static MimeMessage ToMimeMessage(this EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.To.AddRange(message.ToAddresses.Select(x => new MailboxAddress(x.Value)));
            mimeMessage.From.AddRange(message.FromAddresses.Select(x => new MailboxAddress(x.Value)));
            mimeMessage.Cc.AddRange(message.CcAddresses.Select(x => new MailboxAddress(x.Value)));
            mimeMessage.Bcc.AddRange(message.BccAddresses.Select(x => new MailboxAddress(x.Value)));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = message.IsHtml 
                               ? new BodyBuilder { HtmlBody = message.Body }.ToMessageBody() 
                               : new TextPart("plain") { Text = message.Body };
            
            return mimeMessage;
        }

        public static void WithoutSsl(this SmtpClient client)
        {
            client.ServerCertificateValidationCallback = (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
            ;
        }

        public static MailboxAddress ToMailBoxAddress(this string address)
        {
            var emailAdrs = new EmailAddress(address);

            return
                new MailboxAddress(emailAdrs);
        }

        public static IEnumerable<MailboxAddress> ToMailBoxAddresses(this IEnumerable<string> list)
        {
            return
                list.Select(e => e.ToMailBoxAddress());
        }
    }
}
