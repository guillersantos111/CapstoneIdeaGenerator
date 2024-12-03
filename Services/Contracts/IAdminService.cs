using CapstoneIdeaGenerator.Client.Models.DTOs;

namespace CapstoneIdeaGenerator.Client.Services.Contracts
{
    public interface IAdminService
    {
        Task<Response> LoginAsync(AdminLoginDTO request);
        Task<AdminGetByEmailDTO> GetAdminByEmail(string email);
        Task<bool> RegisterAsync(AdminRegisterDTO request);
        Task<Response> ForgotPassword(AdminForgotPasswordDTO request);
        Task<string> ResetPassword(AdminPasswordResetDTO request);
        Task<string> GetAdminNameAsync();
        Task<IEnumerable<AdminAccountDTO>> GetAllAccountsAsync();
        Task<AdminDTO> EditAdminAsync(string email, AdminEditAccountDTO updatedAdmin);
        Task RemoveAdminAsync(string email);
    }
}
