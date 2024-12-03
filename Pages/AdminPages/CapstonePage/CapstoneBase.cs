using CapstoneIdeaGenerator.Client.Services.Contracts;
using CapstoneIdeaGenerator.Client.Models.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using CapstoneIdeaGenerator.Client.Components;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.CapstonePage
{
    public class CapstoneBase : ComponentBase
    {
        public ICollection<CapstonesDTO> Capstones { get; private set; } = new List<CapstonesDTO>();
        private readonly DialogOptions dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };

        [Inject] ICapstoneService CapstoneService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        public bool isLoading { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadCapstones();
        }

        private async Task LoadCapstones()
        {
            try
            {
                isLoading = true;
                var response = await CapstoneService.GetAllCapstones();
                Capstones = response?.ToList() ?? new List<CapstonesDTO>();

                if (response == null)
                {
                    Snackbar.Add("No Capstones Found", Severity.Warning);
                }
                else
                {
                    Capstones = response.ToList();
                    isLoading = false;
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Exception Error: {ex.Message}", Severity.Error);
                NavigationManager.NavigateTo("/home");
            }

            StateHasChanged();
        }


        public async Task AddCapstone()
        {
            var parameters = new DialogParameters<EditCapstoneDialog>();

            var dialog = DialogService.Show<EditCapstoneDialog>("Add Capstone", parameters, dialogOptions);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                Snackbar.Add("Capstone Added Successfully!", Severity.Success);
                await LoadCapstones();
            }
        }

        public async Task UpdateCapstone(CapstonesDTO capstones)
        {
            var parameters = new DialogParameters { ["capstones"] = capstones };
            var dialog = DialogService.Show<EditCapstoneDialog>("Edit Capstone", parameters, dialogOptions);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                Snackbar.Add("Capstone Updated Successfully!", Severity.Success);
                await LoadCapstones();
            }
        }

        public async Task RemoveCapstone(int Id)
        {
            bool? confirm = await DialogService.ShowMessageBox("Delete Confirmation", "Are you sure you want to delete this Capstone?", yesText: "Delete", cancelText: "Cancel");

            if (confirm == true)
            {
                try
                {
                    await CapstoneService.RemoveCapstone(Id);
                    Snackbar.Add("Capstone Removed Successfully!", Severity.Success);
                    await LoadCapstones();
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error removing capstone: {ex.Message}", Severity.Error);
                }
            }
        }
    }
}
