using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace CapstoneIdeaGenerator.Client.Services
{
    public class ActivityLogsService : IActivityLogsService
    {
        private readonly HttpClient httpClient;

        public ActivityLogsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task LogActivity(int adminId, string adminName, string action)
        {
            try
            {
                var activityLog = new ActivityLogsDTO
                {
                    AdminId = adminId,
                    Name = adminName,
                    Actions = action,
                    Timestamp = DateTime.UtcNow
                };

                var response = await httpClient.PostAsJsonAsync("/api/ActivityLogs/post", activityLog);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Activity log successfully recorded.");
                }
                else
                {
                    Console.WriteLine($"Failed to log activity: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging activity: {ex.Message}");
            }
        }



        public async Task<List<ActivityLogsDTO>> GetActivityLogs()
        {
            return await httpClient.GetFromJsonAsync<List<ActivityLogsDTO>>("/api/ActivityLogs/get");
        }
    }
}
