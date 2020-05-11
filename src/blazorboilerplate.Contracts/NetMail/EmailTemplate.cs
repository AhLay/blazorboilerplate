using System;

namespace BlazorBoilerplate.Contracts.NetMail
{
    public class EmailTemplate
    {
        public EmailTemplate(string subjectTemplate,string bodyTemplate)
        {
            if (string.IsNullOrWhiteSpace(subjectTemplate))
            {
                throw new ArgumentException("message", nameof(subjectTemplate));
            }

            if (string.IsNullOrEmpty(bodyTemplate))
            {
                throw new ArgumentException(nameof(bodyTemplate));
            }
            SubjectTemplate = subjectTemplate;
            BodyTemplate = bodyTemplate;
        }
        public string SubjectTemplate { get;}
        public string BodyTemplate { get; }
    }
}