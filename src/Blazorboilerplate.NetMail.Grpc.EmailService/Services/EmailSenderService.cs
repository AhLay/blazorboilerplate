using BlazorBoilerplate.Contracts.NetMail;
using BlazorBoilerplate.NetMail.MailKitEmailService;
using BlazorBoilerplate.Shared;
using BlazorBoilerplate.Shared.Email;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBoilerplate.NetMail.Grpc.EmailService.Services
{
    public class EmailSenderService : EmailSvc.EmailSvcBase
    {
        private readonly IEmailSender<EmailMessage, Result> sender;

        public EmailSenderService(IEmailSender<EmailMessage, Result> sender)
        {
            this.sender = sender;
        }

        public override async Task<Empty> SendEmail(EmailMessageRequest request, ServerCallContext context)
        {
            var emailMessage = request.ToEmailMessage();
            var result = await sender.SendEmail(emailMessage);

            if (result.Failed)
                throw new RpcException(new Status(StatusCode.Internal, result.Message));
            
            return
                new Empty();
        }

    }
}
