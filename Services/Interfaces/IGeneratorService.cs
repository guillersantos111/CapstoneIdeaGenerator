using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Services.Interfaces
{
    public interface IGeneratorService
    {
        Task<IEnumerable<string>> GetAllCategories();
        Task<IEnumerable<string>> GetAllProjectTypes();
        Task<CapstonesDTO> GetByProjectTypeAndCategory(string category, string projectType);
    }
}
