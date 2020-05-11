using System;
using System.Text;

namespace BlazorBoilerplate.Shared.Exceptions
{
    public static class ExceptionExtensions
    {
        public static string ExtractMessages(this Exception exception)
        {
            var sb = new StringBuilder();

            sb.AppendLine(exception.Message);
            var inner = exception.InnerException;

            while (inner != null)
            {
                sb.AppendLine(exception.Message);
                inner = inner.InnerException;
            }

            return sb.ToString();
        }

    }
}
