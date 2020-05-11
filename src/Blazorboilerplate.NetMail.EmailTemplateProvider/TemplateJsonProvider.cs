using BlazorBoilerplate.Contracts.NetMail;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlazorBoilerplate.NetMail.EmailTemplateProvider
{
    public class TemplateJsonProvider : ITemplateProvider
    {
        private readonly TemplateFileProviderOptions options;

        public TemplateJsonProvider(TemplateFileProviderOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<EmailTemplate> GetTemplate(string templateName)
        {
            if (string.IsNullOrWhiteSpace(templateName))
                throw new ArgumentNullException(templateName);

            templateName = $"{templateName}.json";

            var path = Path.Combine(options.TemplateLocationPath, templateName);

            using (StreamReader sr = new StreamReader(path))
            {
                var json = await sr.ReadToEndAsync();

                return JsonConvert.DeserializeObject<EmailTemplate>(json);
            }
        }
    }
}
