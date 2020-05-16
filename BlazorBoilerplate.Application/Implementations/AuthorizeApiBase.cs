using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using BlazorBoilerplate.Shared.Dto;
using System.Collections.Generic;
using BlazorBoilerplate.Shared.Dto.Account;

using static Microsoft.AspNetCore.Http.StatusCodes;
using BlazorBoilerplate.Application.Contracts;
using Microsoft.AspNetCore.Components;

namespace BlazorBoilerplate.Application.Implementations
{
    public abstract class AuthorizeApiBase : IAuthorizeApi
    {
        protected HttpClient HttpClient { get; }
        protected AuthorizeApiBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public abstract Task<ApiResponseDto> Logout();
        public abstract Task<ApiResponseDto> Login(LoginDto loginParameters);

        public virtual async Task<ApiResponseDto> Create(RegisterDto registerParameters)
        {
            return await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/Create", registerParameters);
        }

        public virtual async Task<ApiResponseDto> Register(RegisterDto registerParameters)
        {
            return await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/Register", registerParameters);
        }

        public virtual async Task<ApiResponseDto> ConfirmEmail(ConfirmEmailDto confirmEmailParameters)
        {
            return await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/ConfirmEmail", confirmEmailParameters);
        }

        public virtual async Task<ApiResponseDto> ResetPassword(ResetPasswordDto resetPasswordParameters)
        {
            return await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/ResetPassword", resetPasswordParameters);
        }

        public virtual async Task<ApiResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordParameters)
        {
            return await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/ForgotPassword", forgotPasswordParameters);
        }

        public virtual async Task<UserInfoDto> GetUserInfo()
        {
            UserInfoDto userInfo = new UserInfoDto { IsAuthenticated = false, Roles = new List<string>() };
            ApiResponseDto apiResponse = await HttpClient.GetJsonAsync<ApiResponseDto>("api/Account/UserInfo");

            if (apiResponse.StatusCode == Status200OK)
            {
                userInfo = JsonConvert.DeserializeObject<UserInfoDto>(apiResponse.Result.ToString());
                return userInfo;
            }
            return userInfo;
        }

        public virtual async Task<UserInfoDto> GetUser()
        {
            ApiResponseDto apiResponse = await HttpClient.GetJsonAsync<ApiResponseDto>("api/Account/GetUser");
            UserInfoDto user = JsonConvert.DeserializeObject<UserInfoDto>(apiResponse.Result.ToString());
            return user;
        }

        public virtual async Task<ApiResponseDto> UpdateUser(UserInfoDto userInfo)
        {
            return await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/UpdateUser", userInfo);
        }
    }
}
