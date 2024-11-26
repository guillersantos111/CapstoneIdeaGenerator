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

        public async Task<Response> LoginAsync(LoginRequestDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/login", request);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(token))
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Token is null or empty"
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = token 
                };
            }

            return new Response
            {
                IsSuccess = false,
                Message = "Invalid Credentials"
            };
        }


        public async Task<Response> RegisterAsync(AdminRegisterDTO register)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/Authentication/register", register);

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
                        Message = "Registration failed: " + errorMessage
                    };
                }
            }
            catch (Exception ex)
            {
               
                return new Response
                {
                    IsSuccess = false,
                    Message = "An error occurred during registration: " + ex.Message
                };
            }
        }


        public async Task<string> ForgotPasswordAsync(string email)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Authentication/forgot-password", email);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response>();
                return result?.Token;
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
