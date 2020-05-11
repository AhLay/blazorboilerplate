using BlazorBoilerplate.Contracts.NetMail;
using BlazorBoilerplate.Shared;
using BlazorBoilerplate.Shared.Email;
using MailKit.Net.Smtp;
using System;
using System.Threading.Tasks;

namespace BlazorBoilerplate.NetMail.MailKitEmailService
{
    public class EmailSender : IEmailSender<EmailMessage, Result>
    {
        private readonly EmailSenderOptions options;

        public EmailSender(EmailSenderOptions options)
        {
            this.options = options;
        }

        public async Task<Result> SendEmail(EmailMessage message)
        {
            try
            {                
                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    if (!options.SmtpUseSSL)
                        emailClient.WithoutSsl();                   

                    await emailClient
                        .ConnectAsync(options.SmtpServer, options.SmtpPort, options.SmtpUseSSL)
                        .ConfigureAwait(false);

                    //Remove any OAuth functionality as we won't be using it.
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    if (!string.IsNullOrWhiteSpace(options.SmtpUsername))
                    {
                        await emailClient
                            .AuthenticateAsync(options.SmtpUsername, options.SmtpPassword)
                            .ConfigureAwait(false);
                    }

                    await emailClient
                         .SendAsync(message.ToMimeMessage())
                         .ConfigureAwait(false);

                    await emailClient
                         .DisconnectAsync(true)
                         .ConfigureAwait(false);

                    return
                        Result.Success("Email sent.");
                }
            }
            catch (Exception ex)
            {

                return
                    Result.Error($"smtpHost: {options.SmtpServer}- {ex.Message}");
            }
        }
    }
}
