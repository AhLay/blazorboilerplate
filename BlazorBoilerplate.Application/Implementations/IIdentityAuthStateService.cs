using BlazorBoilerplate.Shared.Dto;
using BlazorBoilerplate.Shared.Dto.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading.Tasks;

namespace BlazorBoilerplate.Application.Implementations
{
    public interface IIdentityAuthStateService
    {
        Task<ApiResponseDto> ConfirmEmail(ConfirmEmailDto confirmEmailParameters);
        Task<ApiResponseDto> Create(RegisterDto registerParameters);
        Task<ApiResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordParameters);
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task<UserInfoDto> GetUserInfo();
        Task<ApiResponseDto> Login(LoginDto loginParameters);
        Task<ApiResponseDto> Logout();
        Task<ApiResponseDto> Register(RegisterDto registerParameters);
        Task<ApiResponseDto> ResetPassword(ResetPasswordDto resetPasswordParameters);
        Task<ApiResponseDto> UpdateUser(UserInfoDto userInfo);
    }
}