using CapstoneIdeaGenerator.Client.Services.Contracts;
using CapstoneIdeaGenerator.Client.Models.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using CapstoneIdeaGenerator.Client.Components;
using Microsoft.AspNetCore.Components.Authorization;
using CapstoneIdeaGenerator.Client.Services;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.CapstonePage
{
    public class CapstoneBase : ComponentBase
    {
        public bool isLoading { get; set; } = false;
        public string SearchQuery { get; set; } = string.Empty;
        public IEnumerable<CapstonesDTO> FilteredCapstones { get; set; } = new List<CapstonesDTO>();
        public ICollection<CapstonesDTO> Capstones { get; private set; } = new List<CapstonesDTO>();
        private readonly DialogOptions dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };

        [Inject] ICapstoneService capstoneService { get; set; }
        [Inject] IDialogService dialogService { get; set; }
        [Inject] ISnackbar snackbar { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }
        [Inject] IActivityLogsService activityLogsService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCapstones();
        }

        private async Task LoadCapstones()
        {
            try
            {
                isLoading = true;
                Capstones = (await capstoneService.GetAllCapstones())?.ToList() ?? new List<CapstonesDTO>();
                FilteredCapstones = Capstones.ToList();
            }
            catch (Exception ex)
            {
                snackbar.Add($"Exception Error: {ex.Message}", Severity.Error);
                navigationManager.NavigateTo("/home");
            }

            isLoading = false;
            StateHasChanged();
        }


        public async Task SearchCapstones(string query)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var filteredResults = await capstoneService.GetFilteredCapstones(query);
                    FilteredCapstones = filteredResults?.ToList() ?? new List<CapstonesDTO>();
                }
                else
                {
                    FilteredCapstones = Capstones.ToList();
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                snackbar.Add($"Error Seatching Capstones: {ex.Message}", Severity.Error);
            }
        }


        public async Task AddCapstone()
        {
            var parameters = new DialogParameters<EditCapstoneDialog>();

            var dialog = dialogService.Show<EditCapstoneDialog>("Add Capstone", parameters, dialogOptions);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                snackbar.Add("Capstone Added Successfully!", Severity.Success);
                await activityLogsService.LogAdminAction("Added Capstone");
                await LoadCapstones();
            }
        }

        public async Task UpdateCapstone(CapstonesDTO capstones)
        {
            var parameters = new DialogParameters { ["capstones"] = capstones };
            var dialog = dialogService.Show<EditCapstoneDialog>("Edit Capstone", parameters, dialogOptions);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                snackbar.Add("Capstone Updated Successfully!", Severity.Success);
                await activityLogsService.LogAdminAction("Updated Capstone");
                await LoadCapstones();
            }
        }

        public async Task RemoveCapstone(int Id)
        {
            bool? confirm = await dialogService.ShowMessageBox("Delete Confirmation", "Are you sure you want to delete this Capstone?", yesText: "Delete", cancelText: "Cancel");

            if (confirm == true)
            {
                try
                {
                    await capstoneService.RemoveCapstone(Id);
                    snackbar.Add("Capstone Removed Successfully!", Severity.Success);
                    await activityLogsService.LogAdminAction("Remove Capstone");
                    await LoadCapstones();
                }
                catch (Exception ex)
                {
                    snackbar.Add($"Error removing capstone: {ex.Message}", Severity.Error);
                }
            }
        }
    }
}
