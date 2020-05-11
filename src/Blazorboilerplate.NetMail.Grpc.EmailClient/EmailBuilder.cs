using BlazorBoilerplate.Contracts.NetMail;
using BlazorBoilerplate.Shared;
using BlazorBoilerplate.Shared.Email;
using System;
using System.Threading.Tasks;

namespace BlazorBoilerplate.NetMail.Grpc.EmailClient
{
    //public interface IEmailBuilder : IWithSubject, IWithBody, IWithFrom, IWithTo, IWithCc, IWithBcc, ISendEmail, IStart
    //{

    //}
    public interface IEmailBuilder
    {
        IWithSubjectTemplate UseTemplate();
        IWithSubject NoTemplate();
    }
    public interface IWithSubjectTemplate : IWithSubject
    {
        IWithBodyTemplate WithSubject<TModel>(TModel model, EmailTemplate template) where TModel : class;
    }

    public interface IWithSubject
    {
        IWithBody WithSubject(string subject);
    }

    public interface IWithBodyTemplate : IWithBody
    {
        IWithFrom WithBody<TModel>(TModel model, EmailTemplate emailTemplate) where TModel : class;

    }

    public interface IWithBody
    {
        IWithFrom WithBody(string body);
    }

    public interface IWithFrom
    {
        IWithTo From(string email);
        IWithTo From(string[] emails);
    }

    public interface IWithTo
    {
        IWithCc To(string email);
        IWithCc To(string[] emails);
    }
    public interface IWithCc : IWithBcc
    {
        IWithBcc Cc(string email);
        IWithBcc Cc(string[] emails);
    }

    public interface IWithBcc : ISendEmail
    {
        ISendEmail Bcc(string email);
        ISendEmail Bcc(string[] emails);
    }

    public interface ISendEmail
    {
        Task<Result> SendEmail();
    }
    
    public class EmailBuilder : IEmailBuilder, IWithSubjectTemplate, IWithBodyTemplate, IWithSubject, IWithBody, IWithFrom, IWithTo, IWithCc, IWithBcc, ISendEmail

    {        
        private readonly EmailMessage _message;
        private readonly IEmailSender<EmailMessage, Result> _emailSender;
        private readonly Func<IEmailTemplateBuilder> _emailTemplateBuilderFactory;
        private readonly IEmailTemplateBuilder _emailTemplateBuilder;

        private EmailBuilder(IEmailSender<EmailMessage, Result> emailSender, EmailMessage emailMessage)
        {
            _emailSender = emailSender;
            _message = emailMessage;
        }

        public EmailBuilder(IEmailSender<EmailMessage, Result> emailSender, Func<IEmailTemplateBuilder> emailTemplateBuilderFactory)
            : this(emailSender, new EmailMessage())
        {
            if (emailTemplateBuilderFactory is null)
                throw new ArgumentNullException(nameof(emailTemplateBuilderFactory));

            this._emailTemplateBuilderFactory = emailTemplateBuilderFactory;
        }

        private EmailBuilder(IEmailSender<EmailMessage, Result> emailSender, EmailMessage emailMessage, IEmailTemplateBuilder emailTemplateBuilder)
            : this(emailSender, emailMessage)
        {
            if (emailTemplateBuilder is null)
                throw new ArgumentNullException(nameof(emailTemplateBuilder));

            _emailTemplateBuilder = emailTemplateBuilder;
        }

        public IWithSubjectTemplate UseTemplate()
        {
            return new EmailBuilder(_emailSender, new EmailMessage(), CreateEmailTemplateBuilder());
        }

        public IWithSubject NoTemplate()
        {
            return new EmailBuilder(_emailSender, new EmailMessage());
        }


        public IWithBody WithSubject(string subject)
        {
            return new EmailBuilder(_emailSender, _message.WithSubject(subject));
        }

        public IWithBodyTemplate WithSubject<TModel>(TModel model, EmailTemplate template)
        where TModel : class
        {
            var builder = CreateEmailTemplateBuilder();

            var subject = builder.CreateSubject(model, template);

            return new EmailBuilder(_emailSender, _message.WithSubject(subject));
        }


        public IWithFrom WithBody(string body)
        {
            return new EmailBuilder(_emailSender, _message
                                                .WithBody(body));
        }


        public IWithFrom WithBody<TModel>(TModel model, EmailTemplate template)
        where TModel : class
        {
            var builder = CreateEmailTemplateBuilder();

            var body = builder.CreateBody(model, template);

            return new EmailBuilder(_emailSender, _message
                                                .WithBody(body));
        }

        public IWithTo From(string[] emails)
        {
            return new EmailBuilder(_emailSender, _message
                                                .WithFromAddress(emails));
        }


        public IWithTo From(string email)
        {
            return
                new EmailBuilder(_emailSender, _message
                                              .WithFromAddress(email));
        }

        public IWithCc To(string email)
        {
            return
            new EmailBuilder(_emailSender, _message
                              .WithToAddress(email));
        }

        public IWithCc To(string[] emails)
        {
            return
            new EmailBuilder(_emailSender, _message
                              .WithToAddress(emails));
        }
        public IWithBcc Cc(string email)
        {
            return
            new EmailBuilder(_emailSender, _message
                              .WithCcAddress(email));
        }

        public IWithBcc Cc(string[] emails)
        {
            return
            new EmailBuilder(_emailSender, _message
                              .WithCcAddress(emails));
        }

        public ISendEmail Bcc(string email)
        {
            return
            new EmailBuilder(_emailSender, _message
                              .WithBccAddress(email));

        }

        public ISendEmail Bcc(string[] emails)
        {
            return
            new EmailBuilder(_emailSender, _message
                              .WithBccAddress(emails));

        }

        public Task<Result> SendEmail()
        {
            if (_emailSender is null)
                return
                     Result.Error("the email sender is not intialized. please call the Configure Method to initialize the EmailSender.")
                           .AsTask();

            return
            _emailSender.SendEmail(_message);
        }

        private IEmailTemplateBuilder CreateEmailTemplateBuilder()
        {
            if (_emailTemplateBuilderFactory is null)
                throw new InvalidOperationException(@$"the EmailTemplateBuilderFactory is not initialized.
                                                      please call the method  Configure(Func<IEmailSender<EmailMessage, Result>> serviceFactory, Func < IEmailTemplateBuilder > templateBuilderFactory)
                                                      to configure the templateBuilder.");

            return _emailTemplateBuilderFactory?.Invoke()
                   ?? throw new ArgumentNullException("EmailTemplateBuilder");
        }
    }
}
