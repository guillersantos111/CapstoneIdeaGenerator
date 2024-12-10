using Blazored.LocalStorage;
using CapstoneIdeaGenerator.Client.Components;
using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using CapstoneIdeaGenerator.Client.Utilities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Pages.AdminPages.LoginPage
{
    public class AuthenticationBase : ComponentBase
    {
        private readonly DialogOptions dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true, NoHeader = true };

        [Inject] public NavigationManager navigationManager { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public IDialogService dialogService { get; set; }
        [Inject] public IAdminService adminService { get; set; }
        [Inject] public IActivityLogsService activityLogsService { get; set; }

        public ShowPasswordUtil showPasswordUtil = new ShowPasswordUtil();
        public ActivityLogsDTO adminLogs = new ActivityLogsDTO();
        public AdminLoginDTO login = new AdminLoginDTO();
        public AdminAccountDTO account = new AdminAccountDTO();
        public string responseMessage = string.Empty;
        public bool isSuccess;
        public bool isLoading = false;

        public async Task LoginOnClick()
        {
            isLoading = true;

            var response = await adminService.LoginAsync(login);

            if (response.IsSuccess)
            {
                isLoading = false;

                var fetchAdmin = await adminService.GetAdminByEmail(login.Email);

                adminLogs = new ActivityLogsDTO
                {
                    AdminId = fetchAdmin.AdminId,
                    Email = fetchAdmin.Email,
                    Name = fetchAdmin.Name,
                    Action = "Logged In",
                    Details = "Admin Successfully Logged In",
                    Timestamp = DateTime.UtcNow
                };

                await activityLogsService.RecordLogsActivity(adminLogs);
                navigationManager.NavigateTo("/dashboard");
            }
            else
            {
                isLoading = false;
                responseMessage = response.Message;
            }
        }


        public void ShowPasswordClick()
        {
            showPasswordUtil.Toggle();
        }


        public async Task OpenForgotPasswordDialog()
        {
            var parameters = new DialogParameters<FotgotPasswordDialog>();

            var dialog = dialogService.Show<FotgotPasswordDialog>("Forgot Password", parameters, dialogOptions);

            var result = await dialog.Result;

            if (!result.Canceled && result.Data is string resetToken)
            {
                snackbar.Add($"Reset Token Successfully Sent: {resetToken}", Severity.Info);
            }
        }
    }
}