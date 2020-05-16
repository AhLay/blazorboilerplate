using System.Threading.Tasks;
using BlazorBoilerplate.Shared.Dto;
using BlazorBoilerplate.Shared.Dto.Account;

namespace BlazorBoilerplate.Application.Contracts
{
    /// <summary>
    /// Access to User Profile information
    /// </summary>
    public interface IUserProfileApi
    {
        Task<ApiResponseDto> Upsert(UserProfileDto userProfile);
        Task<ApiResponseDto> Get();
    }
}
