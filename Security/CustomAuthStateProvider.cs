using Blazored.LocalStorage;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Charts;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace CapstoneIdeaGenerator.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationService _authenticationService;

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient, IAuthenticationService authenticationService)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _authenticationService = authenticationService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "Bearer");

            var user = new ClaimsPrincipal(identity);

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return new AuthenticationState(user);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
            {
                throw new ArgumentException("JWT Is Null Or Empty", nameof(jwt));
            }
            var parts = jwt.Split('.');

            if (parts.Length != 3)
            {
                throw new FormatException("Invalid JWT Format");
            }

            var payload = parts[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())) ??
                Enumerable.Empty<Claim>();
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
            {
                throw new ArgumentException("Base64 String Is Null Or Empty", nameof(base64));
            }
            base64 = base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
            return Convert.FromBase64String(base64);
        }

        public void MarkUserAsAuthenticated(string token)
        {
            if (token.Contains("."))
            {
                var claims = ParseClaimsFromJwt(token);
                var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
            }
            else
            {
                var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, "admin")
            }, "non-jwt"));
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
            }
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
            _localStorageService.RemoveItemAsync("authToken");
            _localStorageService.RemoveItemAsync("authTokenExpiration");
        }

        public string GenerateRandomUserId(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<int?> GetLoggedInAdminId()
        {
            var authState = await GetAuthenticationStateAsync();
            var identity = (ClaimsIdentity)authState.User.Identity;

            if (identity != null)
            {
                var adminIdClaim = identity.FindFirst("AdminId");

                if (adminIdClaim != null && int.TryParse(adminIdClaim.Value, out var adminId))
                {
                    return adminId;
                }
            }

            return null;
        }

    }
}
