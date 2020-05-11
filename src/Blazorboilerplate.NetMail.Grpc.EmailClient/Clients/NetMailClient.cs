using BlazorBoilerplate.Contracts.NetMail;
using BlazorBoilerplate.NetMail.Grpc.EmailService;
using BlazorBoilerplate.Shared;
using BlazorBoilerplate.Shared.Email;
using Grpc.Net.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorBoilerplate.Shared.Exceptions;
namespace BlazorBoilerplate.NetMail.Grpc.EmailClient.Clients
{

    public class NetMailClient : IEmailSender<EmailMessage, Result>
    {

        private readonly EmailClientOptions options;
        private EmailSvc.EmailSvcClient client;
        protected EmailSvc.EmailSvcClient Client
        {
            get
            {
                if (client != null)
                    return client;

                var channel = GrpcChannel.ForAddress(options.ServerUrl);
                client = new EmailSvc.EmailSvcClient(channel);
                return client;
            }
        }

        public NetMailClient(EmailClientOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Result> SendEmail(EmailMessage message)
        {
            try
            {
                var request = CreateRequest(message);
                await Client.SendEmailAsync(request);

                return
                    Result.Success("succeded");

            }
            catch (Exception ex)
            {
                return
                Result.Error(ex.ExtractMessages());
            }

        }

        private EmailMessageRequest CreateRequest(EmailMessage message)
        {
            var request = new EmailMessageRequest
            {
                Subject = message.Subject,
                Body = message.Body,
                IsHtml = message.IsHtml
            };

            request.From.Add(message.FromAddresses.Select(m => m.Value));
            request.To.Add(message.ToAddresses.Select(m => m.Value));
            request.Cc.Add(message.CcAddresses.Select(m => m.Value));
            request.Bcc.Add(message.BccAddresses.Select(m => m.Value));

            return
                request;
        }
    }
}
