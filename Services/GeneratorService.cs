using CapstoneIdeaGenerator.Client.Services.Interfaces;
using CapstoneIdeaGenerator.Client.Models.DTO;
using System.Net.Http.Json;

namespace CapstoneIdeaGenerator.Client.Services
{
    public class GeneratorService : IGeneratorService
    {
        private readonly HttpClient httpClient;

        public GeneratorService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<string>> GetAllCategories()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<string>>("/api/Generator/categories");
        }

        public async Task<IEnumerable<string>> GetAllProjectTypes()
        {
            return await httpClient.GetFromJsonAsync<IEnumerable<string>>("/api/Generator/projectTypes");
        }

        public async Task<CapstonesDTO> GetByProjectTypeAndCategory(string category, string projectType)
        {
            var encodedCategory = Uri.EscapeDataString(category);
            var encodedProjectType = Uri.EscapeDataString(projectType);
            return await httpClient.GetFromJsonAsync<CapstonesDTO>($"/api/Generator/idea/{category}/{projectType}");
        }

    }
}
