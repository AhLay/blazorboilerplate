using System.Threading.Tasks;

namespace BlazorBoilerplate.Contracts.NetMail
{

    public interface IEmailTemplateBuilder
    {
        string CreateBody<TModel>(TModel model, EmailTemplate template) where TModel : class;

        string CreateSubject<TModel>(TModel model, EmailTemplate template) where TModel : class;
    }
}