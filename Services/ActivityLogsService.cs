using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data.SqlTypes;
using System.Net.Http.Json;

namespace CapstoneIdeaGenerator.Client.Services
{
    public class ActivityLogsService : IActivityLogsService
    {
        private readonly HttpClient httpClient;
        private readonly CustomAuthStateProvider customAuthStateProvider;
        private readonly IAdminService adminService;
        private readonly ILogger<ActivityLogsService> logger;

        public ActivityLogsService(HttpClient httpClient, CustomAuthStateProvider customAuthStateProvider, IAdminService adminService, ILogger<ActivityLogsService> logger)
        {
            this.httpClient = httpClient;
            this.customAuthStateProvider = customAuthStateProvider;
            this.adminService = adminService;
            this.logger = logger;
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


        public async Task LogAdminAction(string action)
        {
            var authenticationState = await customAuthStateProvider.GetAuthenticationStateAsync();
            var admin = authenticationState.User;

            if (admin.Identity.IsAuthenticated)
            {
                var logout = new { Email = admin.Identity.Name };
                var fetchadmin = await adminService.GetAdminByEmail(logout.Email);

                if (fetchadmin != null)
                {
                    var adminLogs = new ActivityLogsDTO
                    {
                        AdminId = fetchadmin.AdminId,
                        Email = fetchadmin.Email,
                        Name = fetchadmin.Name,
                        Action = action,
                        Details = $"Admin {fetchadmin.Name} Performed The Action: {action}",
                        Timestamp = DateTime.UtcNow
                    };

                    await RecordLogsActivity(adminLogs);
                }
                else
                {
                    Console.WriteLine($"Admin with email {logout.Email} not found.");
                }
            }
        }
    }
}
