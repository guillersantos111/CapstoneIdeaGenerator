using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using System.Data.SqlTypes;
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

        public async Task RecordLogsActivity(ActivityLogsDTO logs)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/ActivityLogs/log", logs);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("Activity log recorded successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error recording logs: {ex.Message}");
            }
        }



        public async Task<IEnumerable<ActivityLogsDTO>> GetAllActivityLogs()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IEnumerable<ActivityLogsDTO>>("/api/ActivityLogs/getlogs");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Fetching All Logs: {ex.Message}");
                return new List<ActivityLogsDTO>();
            }
        }
    }
}
