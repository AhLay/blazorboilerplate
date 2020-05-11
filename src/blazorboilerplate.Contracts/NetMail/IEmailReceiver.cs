using System.Threading.Tasks;

namespace BlazorBoilerplate.Contracts.NetMail
{
    public interface IEmailReceiver<TRequest,TResponse> 
    {
        Task<TResponse> RecieveEmail(TRequest request);
    }  
}