using ECommerce.UI.Extensions;
using ECommerce.UI.Models;
using ECommerce.UI.Models.Dtos.Auth;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerce.UI.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginDto loginDto)
        {
            var response = await _client.PostAsJson($"/Auth/Login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
                await SignInAsync(loginResponse);
                return true;
            }
            //await ThrowHttpRequestException(response);

            return false;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _client.PostAsJson($"/Auth/Register", registerDto);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.ReadContentAs<bool>();
                return loginResponse;
            }
            await ThrowHttpRequestException(response);

            return false;
        }

        public async Task SignInAsync(LoginResponseDto user)
        {
            var authClaims = new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.Name}"),
                    new Claim("Surname",$"{user.Surname}"),
                    new Claim(ClaimTypes.Email, user.Mail),
                    new Claim("Phone", user.Phone),
                    new Claim("BearerToken", user.Token),
                    new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(authClaims, "Cookies");
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(principal, new AuthenticationProperties
            {
                IsPersistent = false, // Set to true if you want persistent cookie
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Set cookie expiration time
            });
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

        private async Task ThrowHttpRequestException(HttpResponseMessage response)
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            var errorDetails = JsonSerializer.Deserialize<ErrorDetail>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            throw new HttpRequestException(message: errorDetails.ErrorMessage, null, statusCode: (HttpStatusCode)(errorDetails.StatusCode));
        }
    }
}
