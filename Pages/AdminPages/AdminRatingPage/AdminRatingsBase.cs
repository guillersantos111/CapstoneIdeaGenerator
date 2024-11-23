using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.AdminRatingPage
{
    public class AdminRatingsBase : ComponentBase
    {
        public List<RatingsDTO>? Ratings;
        public bool isLoading = false;
        [Inject] IRatingsService RatingsService { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAllRatingsDetailes();
        }

        private async Task LoadAllRatingsDetailes()
        {
            try
            {
                isLoading = true;
                var response = await RatingsService.GetAllRatingsDetailes();
                Ratings = response?.ToList() ?? new List<RatingsDTO>();

                if (response == null)
                {
                    Snackbar.Add("No Ratings Found", Severity.Error);
                }
                else
                {
                    Ratings = response.ToList();
                    isLoading = false;
                }
            }
            catch (HttpRequestException ex)
            {
                Snackbar.Add($"HTTPS Request Error: {ex.Message}");
            }

            StateHasChanged();
        }
    }
}
