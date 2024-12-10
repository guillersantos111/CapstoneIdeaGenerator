using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using CapstoneIdeaGenerator.Client.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.AdminRatingPage
{
    public class AdminRatingsBase : ComponentBase
    {
        public List<RatingsDTO>? Ratings;
        public bool isLoading = false;
        [Inject] IRatingsService ratingsService { get; set; }
        [Inject] ISnackbar snackbar { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] IActivityLogsService activityLogsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAllRatingsDetailes();
        }

        private async Task LoadAllRatingsDetailes()
        {
            try
            {
                isLoading = true;
                var response = await ratingsService.GetAllRatingsDetailes();
                Ratings = response?.ToList() ?? new List<RatingsDTO>();

                if (response != null)
                {
                    await activityLogsService.LogAdminAction("Viewed Ratings");
                }

                if (response == null)
                {
                    snackbar.Add("No Ratings Found", Severity.Error);
                }
                else
                {
                    Ratings = response.ToList();
                    isLoading = false;
                }
            }
            catch (Exception ex)
            {
                snackbar.Add($"Exception Error: {ex.Message}", Severity.Error);
                navigationManager.NavigateTo("/home");
            }

            StateHasChanged();
        }
    }
}
