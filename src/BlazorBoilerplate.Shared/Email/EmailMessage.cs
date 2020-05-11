using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BlazorBoilerplate.Shared.Email
{
    public class EmailMessage
    {
        public EmailMessage(bool isHtml = true)
        : this(string.Empty, string.Empty, isHtml
         , Enumerable.Empty<EmailAddress>()
         , Enumerable.Empty<EmailAddress>()
         , Enumerable.Empty<EmailAddress>()
         , Enumerable.Empty<EmailAddress>()
         , false)
        {

        }


        public EmailMessage(string subject, string body, bool isHtml = true)
            : this(subject, body, isHtml
                 , Enumerable.Empty<EmailAddress>()
                 , Enumerable.Empty<EmailAddress>()
                 , Enumerable.Empty<EmailAddress>()
                 , Enumerable.Empty<EmailAddress>()
                 , false)
        {

        }

        private EmailMessage(string subject, string body, bool isHtml
            , IEnumerable<EmailAddress> toAddresses
            , IEnumerable<EmailAddress> fromAddresses
            , IEnumerable<EmailAddress> ccAddresses
            , IEnumerable<EmailAddress> bccAddresses
            , bool isEmpty = false)
        {

            Subject = subject;
            Body = body;
            IsHtml = isHtml;
            IsEmpty = isEmpty;

            this.ToAddresses = toAddresses.ToImmutableHashSet();
            this.FromAddresses = fromAddresses.ToImmutableHashSet();
            this.CcAddresses = ccAddresses.ToImmutableHashSet();
            this.BccAddresses = bccAddresses.ToImmutableHashSet();
        }


        public static EmailMessage Empty => new EmailMessage("", "", false
                 , Enumerable.Empty<EmailAddress>()
                 , Enumerable.Empty<EmailAddress>()
                 , Enumerable.Empty<EmailAddress>()
                 , Enumerable.Empty<EmailAddress>()
                 , true);

        public bool IsEmpty { get; }
        public bool IsHtml { get; }
        public ImmutableHashSet<EmailAddress> ToAddresses { get; }
        public ImmutableHashSet<EmailAddress> FromAddresses { get; }
        public ImmutableHashSet<EmailAddress> BccAddresses { get; }
        public ImmutableHashSet<EmailAddress> CcAddresses { get; }
        public string Subject { get; }
        public string Body { get; }

        public EmailMessage WithSubject(string subject)
        {
            if (!IsEmpty && string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));

            return
                new EmailMessage(subject, Body, IsHtml
                , ToAddresses
                , FromAddresses
                , CcAddresses
                , BccAddresses);
        }

        public EmailMessage WithBody(string body)
        {
            if (!IsEmpty && string.IsNullOrWhiteSpace(body))
                throw new ArgumentNullException(nameof(body));

            return
                new EmailMessage(Subject, body, IsHtml
                , ToAddresses
                , FromAddresses
                , CcAddresses
                , BccAddresses);
        }


        public EmailMessage WithToAddress(string[] addresses) => new EmailMessage(Subject, Body, IsHtml
                , addresses.ToEmailAddresses()
                , FromAddresses
                , CcAddresses
                , BccAddresses);

        public EmailMessage WithToAddress(string addresse) => new EmailMessage(Subject, Body, IsHtml
                , ToAddresses.Add(addresse.ToEmailAddress())
                , FromAddresses
                , CcAddresses
                , BccAddresses);

        public EmailMessage WithFromAddress(string address) => new EmailMessage(Subject, Body, IsHtml
                , ToAddresses
                , FromAddresses.Add(address.ToEmailAddress())
                , CcAddresses
                , BccAddresses);

        public EmailMessage WithFromAddress(string[] addresses) => new EmailMessage(Subject, Body, IsHtml
                , ToAddresses
                , addresses.ToEmailAddresses()
                , CcAddresses
                , BccAddresses);

        public EmailMessage WithCcAddress(params string[] addresses) => new EmailMessage(Subject, Body, IsHtml
                , ToAddresses
                , FromAddresses
                , addresses.ToEmailAddresses().ToList()
                , BccAddresses);

        public EmailMessage WithCcAddress(string address) => new EmailMessage(Subject, Body, IsHtml
                , ToAddresses
                , FromAddresses
                , CcAddresses.Add(address.ToEmailAddress())
                , BccAddresses);


        public EmailMessage WithBccAddress(params string[] addresses) => new EmailMessage(Subject, Body, IsHtml
                , ToAddresses
                , FromAddresses
                , CcAddresses
                , addresses.ToEmailAddresses());

        public EmailMessage WithBccAddress(string address) => new EmailMessage(Subject, Body, IsHtml
                , ToAddresses
                , FromAddresses
                , CcAddresses
                , BccAddresses.Add( address.ToEmailAddress()));

    }
}
