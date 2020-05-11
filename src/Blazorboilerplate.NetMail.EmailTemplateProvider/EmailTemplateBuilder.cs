using BlazorBoilerplate.Contracts.NetMail;
using FastMember;
using System;
using System.Threading.Tasks;

namespace BlazorBoilerplate.NetMail.EmailTemplateProvider
{

    public class EmailTemplateBuilder : IEmailTemplateBuilder
    {
        public string CreateBody<TModel>(TModel model, EmailTemplate template)
        where TModel : class
        {
            if (template is null)
                throw new ArgumentNullException(nameof(template));

            return
                ReplaceFoundProperties(model, template.BodyTemplate);
        }

        public string CreateSubject<TModel>(TModel model, EmailTemplate template)
        where TModel : class
        {
            if (template is null)
                throw new ArgumentNullException(nameof(template));

            return
                ReplaceFoundProperties(model, template.SubjectTemplate);
        }


        private string ReplaceFoundProperties<TModel>(TModel model, string template)
        {
            var accessor = TypeAccessor.Create(typeof(TModel));

            var properties = accessor.GetMembers();
            var result = template;
            foreach (var p in properties)
            {
                result = result.Replace($"{{{p.Name}}}", accessor[model, p.Name]?.ToString());
            }

            return
                result;
        }
    }
}
