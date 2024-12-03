using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using CapstoneIdeaGenerator.Client.Utilities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Runtime.CompilerServices;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.AccountPage
{
    public class AccountBase : ComponentBase
    {
        [Inject] IActivityLogsService activityLogsService { get; set; }
        [Inject] IAdminService adminService { get; set; }
        [Inject] IDialogService dialogService { get; set; }
        [Inject] ISnackbar snackbar { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }

        public ICollection<AdminAccountDTO> Admins { get; set; } = new List<AdminAccountDTO>();
        public static AdminRegisterDTO register = new AdminRegisterDTO();
        public ActivityLogsDTO adminLogs = new ActivityLogsDTO();
        public AdminLoginDTO adminLoggedIn = new AdminLoginDTO();
        public List<AdminAccountDTO> accounts = new List<AdminAccountDTO>();
        public ShowPasswordUtil showPassword { get; set; } = new ShowPasswordUtil();
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
                var response = await adminService.GetAllAccountsAsync();
                Admins = response?.ToList() ?? new List<AdminAccountDTO>();

                if (response == null)
                {
                    snackbar.Add("No Account Found", Severity.Warning);
                }
                else
                {
                    Admins = response.ToList();
                }
            }
            catch (Exception ex)
            {
                snackbar.Add($"Exception Error: {ex.Message}", Severity.Error);
                navigationManager.NavigateTo("/home");
            }

            StateHasChanged();
        }

        public async Task RegisterOnClick()
        {
            isLoading = true;
            var response = await adminService.RegisterAsync(register);
            if (response)
            {
                isLoading = false;

                snackbar.Add("Successfully Created Admin Account", Severity.Success);
            }
            else
            {
                snackbar.Add("Failed Registrating Admin Account", Severity.Error);
            }

            await LoadAccounts();
        }



        public async Task EditAdmin(string email)
        {

        }


       public async Task RemoveAccount(string email)
       {
          bool? confirm = await dialogService.ShowMessageBox("Delete Confirmation", "Are you sure you want to delete this Account?", yesText: "Delete", cancelText: "Cancel");

           if (confirm == true)
           {
               await adminService.RemoveAdminAsync(email);
               snackbar.Add("Account Remove Successfully", Severity.Success);
               await LoadAccounts();
           }
       }

        public void ShowPassword()
        {
            showPassword.Toggle();
        }
    }
}
