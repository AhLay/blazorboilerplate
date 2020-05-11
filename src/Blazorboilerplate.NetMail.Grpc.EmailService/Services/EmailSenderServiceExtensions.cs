using BlazorBoilerplate.Shared.Email;
using System.Linq;

namespace BlazorBoilerplate.NetMail.Grpc.EmailService.Services
{
    public static class EmailSenderServiceExtensions
    {
        public static EmailMessage ToEmailMessage(this EmailMessageRequest request)
        {
            return
            new EmailMessage(request.Subject, request.Body, request.IsHtml)
                               .WithFromAddress(request.From.ToArray())
                               .WithToAddress(request.To.ToArray())
                               .WithCcAddress(request.Cc.ToArray())
                               .WithBccAddress(request.Bcc.ToArray());
        }
    }
}
