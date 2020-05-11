using System;

namespace BlazorBoilerplate.NetMail.MailKitEmailService
{
    public class EmailSenderOptions
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; } = string.Empty;
        public string SmtpPassword { get; } = string.Empty;
        public bool SmtpUseSSL { get;  }
    }
}
