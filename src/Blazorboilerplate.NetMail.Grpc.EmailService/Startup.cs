using BlazorBoilerplate.Contracts.NetMail;
using BlazorBoilerplate.NetMail.Grpc.EmailService.Services;
using BlazorBoilerplate.NetMail.MailKitEmailService;
using BlazorBoilerplate.Shared;
using BlazorBoilerplate.Shared.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorBoilerplate.NetMail.Grpc.EmailService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender<EmailMessage, Result>, EmailSender>(sp =>
            {
                var options = sp.GetService<IConfiguration>()
                                .GetSection(nameof(EmailSenderOptions))
                                .Get<EmailSenderOptions>();

                return new EmailSender(options);

            });
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EmailSenderService>();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }
}
