using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Services.Interfaces
{
    public interface IRatingsService
    {
        Task<bool> SubmitRating(RatingRequestDTO ratingRequestDTO);
        Task<List<RatingRequestDTO>> GetAllRatings();
        Task<List<RatingsDTO>> GetAllRatingsDetailes();
    }
}
