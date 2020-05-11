using System;

namespace BlazorBoilerplate.Shared.Exceptions
{

    public class InvalidEmailAddressException : Exception
    {
        public InvalidEmailAddressException(string message) : base(message)
        {

        }
    }
}
