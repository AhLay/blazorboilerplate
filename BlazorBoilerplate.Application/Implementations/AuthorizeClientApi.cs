using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using BlazorBoilerplate.Shared.Dto;
using BlazorBoilerplate.Shared.Dto.Account;
using BlazorBoilerplate.Application.Contracts;

namespace BlazorBoilerplate.Application.Implementations
{
    public class AuthorizeClientApi : AuthorizeApiBase
    {
        public AuthorizeClientApi(HttpClient httpClient): base(httpClient)
        {
        }

        public override async Task<ApiResponseDto> Login(LoginDto loginParameters)
        {
            ApiResponseDto resp;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/Account/Login");
            httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(loginParameters));
            httpRequestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            using (var response = await HttpClient.SendAsync(httpRequestMessage))
            {
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ApiResponseDto>(content);
            }

            return resp;
        }

        public override async Task<ApiResponseDto> Logout()
        {
            var resp = await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/Logout", null);
            return resp;
        }


    }
}
