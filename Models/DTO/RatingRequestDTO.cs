namespace CapstoneIdeaGenerator.Client.Models.DTO
{
    public class RatingRequestDTO
    {
        public int CapstoneId { get; set; }
        public string Title { get; set; }
        public int RatingValue { get; set; }
        public string UserId { get; set; }
    }
}
