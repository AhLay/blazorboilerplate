using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

using BlazorBoilerplate.Shared.Dto;
using System.Collections.Generic;
using BlazorBoilerplate.Shared.Dto.Account;
using Microsoft.JSInterop;

using static Microsoft.AspNetCore.Http.StatusCodes;
using System.Net;
using System.Linq;

namespace BlazorBoilerplate.Application.Implementations
{
    public class AuthorizeServerApi : AuthorizeApiBase
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthorizeServerApi(HttpClient httpClient, IJSRuntime jsRuntime) : base(httpClient)
        {            
            _jsRuntime = jsRuntime;
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


                if (response.Headers.TryGetValues("Set-Cookie", out var cookieEntries))
                {
                    var uri = response.RequestMessage.RequestUri;
                    var cookieContainer = new CookieContainer();

                    foreach (var cookieEntry in cookieEntries)
                    {
                        cookieContainer.SetCookies(uri, cookieEntry);
                    }

                    var cookies = cookieContainer.GetCookies(uri).Cast<Cookie>();


                    foreach (var cookie in cookies)
                    {
                        await _jsRuntime.InvokeVoidAsync("jsInterops.setCookie", cookie.ToString());
                    }
                }

                var content = await response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<ApiResponseDto>(content);
            }

            return resp;
        }

        public override async Task<ApiResponseDto> Logout()
        {
            List<string> cookies = null;
            if (HttpClient.DefaultRequestHeaders.TryGetValues("Cookie", out IEnumerable<string> cookieEntries))
                cookies = cookieEntries.ToList();

            var resp = await HttpClient.PostJsonAsync<ApiResponseDto>("api/Account/Logout", null);

            if (resp.StatusCode == Status200OK && cookies != null && cookies.Any())
            {
                HttpClient.DefaultRequestHeaders.Remove("Cookie");

                foreach (var cookie in cookies[0].Split(';'))
                {
                    var cookieParts = cookie.Split('=');
                    await _jsRuntime.InvokeVoidAsync("jsInterops.removeCookie", cookieParts[0]);
                }
            }

            return resp;
        }
    }
}
