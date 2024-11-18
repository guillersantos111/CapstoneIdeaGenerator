using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(LoginRequestDTO request);
        Task<Response> RegisterAsync(AdminDTO admin);
        Task<string> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(PasswordResetRequestDTO request);
        Task<IEnumerable<AdminDTO>> GetAllAccounts();
        Task RemoveAccount(int id);
    }
}
