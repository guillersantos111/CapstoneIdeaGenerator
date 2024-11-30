using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response> LoginAsync(AdminLoginDTO request);
        Task<bool> RegisterAsync(AdminRegisterDTO request);
        Task<Response> ForgotPassword(AdminForgotPasswordDTO request);
        Task<string> ResetPassword(AdminPasswordResetDTO request);
        Task<string> GetAdminNameAsync();
        Task<IEnumerable<AdminAccountDTO>> GetAllAccountsAsync();
        Task<AdminDTO> EditAdminAsync(string email, AdminEditAccountDTO updatedAdmin);
        Task RemoveAdminAsync(string email);
    }
}
