using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.AdminDashboard
{
    public class DashboardBase : ComponentBase
    {
        [Inject] IActivityLogsService activityLogsService { get; set; }
        public List<ActivityLogsDTO> activityLogs = new List<ActivityLogsDTO>();

        protected override async Task OnInitializedAsync()
        {
            activityLogs = await activityLogsService.GetActivityLogs();
        }
    }
}
