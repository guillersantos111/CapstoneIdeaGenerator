using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.AccountPage
{
    public class AccountBase : ComponentBase
    {
        public ICollection<AdminDTO> Admins { get; set; } = new List<AdminDTO>();

        [Inject] IAuthenticationService authenticationService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }

        public static AdminDTO admin = new AdminDTO();
        public static AdminRegisterDTO register = new AdminRegisterDTO();
        public string message = string.Empty;
        public bool isLoading = false;
        public MudForm form;

        protected override async Task OnInitializedAsync()
        {
            await LoadAccounts();
        }

        public async Task LoadAccounts()
        {
            try
            {
                var response = await authenticationService.GetAllAccounts();
                Admins = response?.ToList() ?? new List<AdminDTO>();

                if (response == null)
                {
                    Snackbar.Add("No Account Found", Severity.Warning);
                }
                else
                {
                    Admins = response.ToList();
                }
            }
            catch (HttpRequestException httpEx)
            {
                Snackbar.Add($"HTTPS Request Error: {httpEx}", Severity.Error);
            }

            StateHasChanged();
        }

        public async Task RegisterOnClick()
        {
            isLoading = true;
            var response = await authenticationService.RegisterAsync(register);
            if (response.IsSuccess)
            {
                isLoading = false;

                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
                Snackbar.Add(response.Message, Severity.Success);
            }
            else
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
                Snackbar.Add(response.Message, Severity.Error);
            }

            StateHasChanged();
        }

        public async Task RemoveAccount(int id)
        {
            bool? confirm = await DialogService.ShowMessageBox("Delete Confirmation", "Are you sure you want to delete this Account?", yesText: "Delete", cancelText: "Cancel");

            if (confirm == true)
            {
                await authenticationService.RemoveAccount(id);
                Snackbar.Add("Account Remove Successfully", Severity.Success);
                await LoadAccounts();
            }
        }

        public void ResetFormFields()
        {
            admin = new AdminDTO();
            form.ResetAsync();
        }
    }
}
