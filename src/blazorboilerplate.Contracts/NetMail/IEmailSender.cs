using System.Threading.Tasks;

namespace BlazorBoilerplate.Contracts.NetMail
{
    public interface IEmailSender<TMessage,TResult> 
    {
        Task<TResult> SendEmail(TMessage message);
    }
}