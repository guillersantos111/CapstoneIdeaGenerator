namespace CapstoneIdeaGenerator.Client.Models.DTO
{
    public class ActivityLogsDTO
    {
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Actions { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
