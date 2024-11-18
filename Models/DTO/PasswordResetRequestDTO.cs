namespace CapstoneIdeaGenerator.Client.Models.DTO
{
    public class PasswordResetRequestDTO
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
