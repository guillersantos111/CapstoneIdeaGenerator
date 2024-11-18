using System.Text.Json.Serialization;

namespace CapstoneIdeaGenerator.Client.Models.DTO
{
    public class LoginRequestDTO
    {
        [JsonRequired]
        public string Email { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }
}
