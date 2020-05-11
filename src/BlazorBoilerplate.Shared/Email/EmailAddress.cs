using BlazorBoilerplate.Shared.Exceptions;
using System.Text.RegularExpressions;

namespace BlazorBoilerplate.Shared.Email
{

    public class ContactEmail : EmailAddress
    {
        public ContactEmail(string name,string value) : base(value)
        {
            Name = name;
        }

        public string Name { get; }

    }
    public class EmailAddress
    {
        public EmailAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailAddressException("email address vlaue is null.");

            if (!IsValidEmailAddress(value))
                throw new InvalidEmailAddressException($"the given email address [{value}] is invalid");

            Value = value;
        }

        public static EmailAddress From(string address)
        {
            return new EmailAddress(address);
        }

        private bool IsValidEmailAddress(string value)
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";


            return
            Regex.IsMatch(value, validEmailPattern, RegexOptions.IgnoreCase);
        }
        public string Value { get; }

        public static implicit operator string(EmailAddress address) => address.Value;
        public static explicit operator EmailAddress(string value) => new EmailAddress(value);

        public override string ToString()
        {
            return Value;
        }
    }
}
