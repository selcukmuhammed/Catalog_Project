using Microsoft.AspNetCore.Authentication;
using MVCatalog.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MVCatalog.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly string _url;

        public AuthService(IConfiguration config)
        {
            _configuration = config;
            _httpClient = new HttpClient();
            _httpContextAccessor = new HttpContextAccessor();
            _url = _configuration.GetValue<string>("URL:Api");
            _httpClient.BaseAddress = new Uri(_url);
        }
        public async Task<string> LoginAsync(LoginModel loginModel)
        {
            using var response = new HttpRequestMessage(new HttpMethod("POST"), _url + "/authentication/Login");
            response.Content = new StringContent(JsonConvert.SerializeObject(new { loginModel.UserName, loginModel.Password }));
            response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var request = _httpClient.Send(response);

            if (!request.IsSuccessStatusCode)
                return null;

            var token = await request.Content.ReadAsStringAsync();

            return token;
        }
        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("token");
        }
    }
}
