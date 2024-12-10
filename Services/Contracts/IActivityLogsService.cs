using CapstoneIdeaGenerator.Client.Models.DTOs;

namespace CapstoneIdeaGenerator.Client.Services.Contracts
{
    public interface IActivityLogsService
    {
        Task RecordLogsActivity(ActivityLogsDTO logs);
        Task<IEnumerable<ActivityLogsDTO>> GetAllActivityLogs();
        Task LogAdminAction(string action);
    }
}
