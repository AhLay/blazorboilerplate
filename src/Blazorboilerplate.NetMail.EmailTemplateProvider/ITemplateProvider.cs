using BlazorBoilerplate.Contracts.NetMail;
using System.Threading.Tasks;

namespace BlazorBoilerplate.NetMail.EmailTemplateProvider
{
    public interface ITemplateProvider
    {
        Task<EmailTemplate> GetTemplate(string templateName);
    }
}
