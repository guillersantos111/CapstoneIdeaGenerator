using CapstoneIdeaGenerator.Client.Models.DTOs;

namespace CapstoneIdeaGenerator.Client.Services.Contracts

{
    public interface ICapstoneService
    {
        Task<IEnumerable<CapstonesDTO>> GetAllCapstones();
        Task<CapstonesDTO> GetCapstoneById(int id);
        Task AddCapstone(CapstonesDTO capstones);
        Task UpdateCapstone(int id, CapstonesDTO capstones);
        Task RemoveCapstone(int id);
    }
}
