using BlazorBoilerplate.Shared.Email;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorBoilerplate.Shared.Email
{
    public static class EmailAddressExtensions
    {
        public static IEnumerable<EmailAddress> ToEmailAddresses(this string[] values)
        {
            if (values is null || !values.Any())
                return Array.Empty<EmailAddress>();

            return values.Select(v=> (EmailAddress)v);
        }

        public static EmailAddress ToEmailAddress(this string value)
        {
            return EmailAddress.From(value);
        }

    }
}
