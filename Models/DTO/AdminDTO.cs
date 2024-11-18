using System.Text.Json.Serialization;

namespace CapstoneIdeaGenerator.Client.Models.DTO
{
    public class AdminDTO
    {
        public int AdminId { get; set; }

        [JsonRequired]
        public string Name { get; set; } = string.Empty;

        [JsonRequired]
        public string Gender { get; set; } = string.Empty;

        [JsonRequired]
        public string Age { get; set; } = string.Empty;

        [JsonRequired]
        public string Email { get; set; } = string.Empty;

        [JsonRequired]
        public string Password { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
