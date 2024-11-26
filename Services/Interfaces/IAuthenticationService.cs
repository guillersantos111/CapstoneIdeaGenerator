using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response> LoginAsync(LoginRequestDTO request);
        Task<Response> RegisterAsync(AdminRegisterDTO register);
        Task<string> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(PasswordResetRequestDTO request);
        Task<IEnumerable<AdminDTO>> GetAllAccounts();
        Task RemoveAccount(int id);
    }
}
