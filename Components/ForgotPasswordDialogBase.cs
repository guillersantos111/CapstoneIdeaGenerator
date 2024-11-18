using Blazored.LocalStorage;
using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class ForgotPasswordDialogBase : ComponentBase
    {
        [Parameter] public ForgotPasswordRequestDTO request { get; set; } = new ForgotPasswordRequestDTO();
        public string errorMessage = string.Empty;
        public string successMessage = string.Empty;

        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public IAuthenticationService AuthService { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }

        public async Task ForgotPasswordDialogOnClick()
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                errorMessage = "Email is required.";
                Snackbar.Add(errorMessage, Severity.Error);
                return;
            }

            try
            {
                var response = await AuthService.ForgotPasswordAsync(request.Email);

                if (!string.IsNullOrEmpty(response) && response != "Error")
                {
                    request.Token = response;
                    successMessage = "Password reset token has been generated.";
                    Snackbar.Add(successMessage, Severity.Success);
                }
                else
                {
                    errorMessage = response ?? "Failed to generate reset token. Please try again.";
                    Snackbar.Add(errorMessage, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
                Snackbar.Add(errorMessage, Severity.Error);
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        public async Task OpenResetPasswordDialog()
        {
            var parameter = new DialogParameters<ResetPasswordDialog>();
            var dialogOptions = new DialogOptions { CloseButton = true, FullWidth = true };

            var dialog = DialogService.Show<ResetPasswordDialog>("Reset Password", parameter, dialogOptions);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                Snackbar.Add("Reset Password Successfully ", Severity.Success, options => options.VisibleStateDuration = 4000);
            }
        }
    }

}
