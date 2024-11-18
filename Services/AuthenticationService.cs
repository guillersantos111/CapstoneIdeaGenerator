using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(LoginRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/login", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }


        public async Task<Response> RegisterAsync(AdminDTO admin)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/register", admin);

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                return new Response
                {
                    IsSuccess = true,
                    Message = message
                };
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new Response
                {
                    IsSuccess = false,
                    Message = errorMessage
                };
            }
        }


        public async Task<string> ForgotPasswordAsync(string email)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/forgot-password", email);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response>();
                return result?.Token; // Return the token directly
            }

            var error = await response.Content.ReadAsStringAsync();
            return $"Error: {error}";
        }


        public async Task<bool> ResetPasswordAsync(PasswordResetRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/reset-password", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<AdminDTO>> GetAllAccounts()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<AdminDTO>>("/api/Authentication/accounts") ?? new List<AdminDTO>();
        }

        public async Task RemoveAccount(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Authentication/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
