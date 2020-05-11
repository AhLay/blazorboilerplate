using BlazorBoilerplate.Contracts.NetMail;
using Microsoft.Extensions.Configuration;

namespace BlazorBoilerplate.Server.Helpers
{
    public static class EmailTemplatesExtensions
    {
        public static EmailTemplate GetTemplate(this IConfiguration config,string templateName)
        {
            return
                config.GetSection(templateName)
                .Get<EmailTemplate>();
        }
    }
}
