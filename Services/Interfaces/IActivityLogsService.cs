using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Services.Interfaces
{
    public interface IActivityLogsService
    {
        Task LogActivity(int adminId, string adminName, string action);
        Task<List<ActivityLogsDTO>> GetActivityLogs();
    }
}
