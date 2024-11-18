namespace CapstoneIdeaGenerator.Client.Models.DTO
{
    public class RatingsDTO
    {
        public int RatingId { get; set; }
        public int CapstoneId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public DateTime RatedOn { get; set; }
        public int RatingValue { get; set; }
    }
}
