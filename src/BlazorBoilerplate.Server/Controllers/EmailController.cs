using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlazorBoilerplate.NetMail.Grpc.EmailClient;
using BlazorBoilerplate.Shared.Dto.Email;
using BlazorBoilerplate.Server.Middleware.Wrappers;
using static Microsoft.AspNetCore.Http.StatusCodes;
using System.Threading.Tasks;

namespace BlazorBoilerplate.Server.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailBuilder _emailBuilder;
        public EmailController(IEmailBuilder emailBuilder)
        {
            _emailBuilder = emailBuilder;
        }

        [HttpPost("Send")]
        [ProducesResponseType((int)Status200OK)]
        [ProducesResponseType((int)Status400BadRequest)]
        public async Task<ApiResponse> Send(EmailDto parameters)
        {
            if (ModelState.IsValid)
            {
                var emailResult = await _emailBuilder
                .UseTemplate()
                .WithSubject(parameters.Subject)
                .WithBody(parameters.Body)
                .From(parameters.FromAddress)
                .To(parameters.ToAddress)
                .SendEmail();

                return emailResult.Failed
                    ? new ApiResponse(Status500InternalServerError, emailResult.Message)
                    : new ApiResponse(Status200OK, emailResult.Message);
            }
            
            return
            new ApiResponse(Status400BadRequest, "User Model is Invalid");
        }
        //[HttpGet("Receive")]
        //[Authorize]
        //public async Task<ApiResponse> Receive()
        //    => await _emailManager.ReceiveMailImapAsync();
    }
}
