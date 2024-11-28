using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.AccountPage
{
    public class AccountBase : ComponentBase
    {
        [Inject] IAuthenticationService authenticationService { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        public ICollection<AccountDTO> Admins { get; set; } = new List<AccountDTO>();
        public static AdminRegisterDTO register = new AdminRegisterDTO();
        public List<AccountDTO> accounts = new List<AccountDTO>();
        public MudForm form;
        public string message = string.Empty;
        public bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadAccounts();
        }

        public async Task LoadAccounts()
        {
            try
            {
                var response = await authenticationService.GetAllAccountsAsync();
                Admins = response?.ToList() ?? new List<AccountDTO>();

                if (response == null)
                {
                    Snackbar.Add("No Account Found", Severity.Warning);
                }
                else
                {
                    Admins = response.ToList();
                }
            }
            catch (HttpRequestException ex)
            {
                Snackbar.Add($"HTTPS Request Error: {ex.Message}");
                NavigationManager.NavigateTo("/home");
            }

            StateHasChanged();
        }

        public async Task RegisterOnClick()
        {
            isLoading = true;
            var response = await authenticationService.RegisterAsync(register);
            if (response)
            {
                isLoading = false;

                Snackbar.Add("Successfully Created Admin Account", Severity.Success);
            }
            else
            {
                Snackbar.Add("Failed Registrating Admin Account", Severity.Error);
            }

            await LoadAccounts();
        }

       // public async Task RemoveAccount(int id)
       // {
          // bool? confirm = await DialogService.ShowMessageBox("Delete Confirmation", "Are you sure you want to delete this Account?", yesText: "Delete", cancelText: "Cancel");

           // if (confirm == true)
           // {
           //    await authenticationService.RemoveAccount(id);
           //    Snackbar.Add("Account Remove Successfully", Severity.Success);
           //    await LoadAccounts();
           // }
        // }
    }
}
