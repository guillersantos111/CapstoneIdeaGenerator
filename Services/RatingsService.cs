using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Pages.RatingsPage;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace CapstoneIdeaGenerator.Client.Services
{
    public class RatingsService : IRatingsService
    {
        private readonly HttpClient httpClient;

        public RatingsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> SubmitRating(RatingRequestDTO ratingRequest)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Ratings/submit", ratingRequest);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error submitting rating: {ex.Message}");
                return false;
            }
        }


        public async Task<List<RatingRequestDTO>> GetAllRatings()
        {
            var response = await httpClient.GetAsync("/api/Ratings");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<RatingRequestDTO>>() ?? new List<RatingRequestDTO>();
        }


        public async Task<List<RatingsDTO>> GetAllRatingsDetailes()
        {
            var response = await httpClient.GetAsync("/api/Ratings/allDetailes");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<RatingsDTO>>() ?? new List<RatingsDTO>();
        }
    }
}
