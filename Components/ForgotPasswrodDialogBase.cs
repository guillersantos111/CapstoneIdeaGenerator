using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using CapstoneIdeaGenerator.Client.Utilities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class ForgotPasswordDialogBase : ComponentBase
    {
        [Parameter] public AdminForgotPasswordDTO AdminForgot { get; set; } = new AdminForgotPasswordDTO();
        public readonly DialogOptions dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };
        [CascadingParameter] MudDialogInstance mudDialog { get; set; }
        [Inject] IAdminService authenticationService { get; set; }
        [Inject] IClipboardService clipboardService { get; set; }
        [Inject] IDialogService dialogService { get; set; }

        public string ResetToken { get; set; }
        public string ErrorMessage { get; set; }

        public async Task Submit()
        {
            try
            {
                var response = await authenticationService.ForgotPassword(AdminForgot);

                if (!string.IsNullOrWhiteSpace(response?.Token))
                {
                    ResetToken = response.Token;
                    ErrorMessage = string.Empty;
                }
                else
                {
                    throw new Exception("Failed To Generate Reset Token");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }


        public async Task CopyTokenToClipboard()
        {
            await clipboardService.CopyText(ResetToken);

            var parameters = new DialogParameters()
            {
                { "Token", ResetToken },
                { "adminPasswordReset", new AdminPasswordResetDTO() }
            };

            var dialog = dialogService.Show<ResetPasswordDialog>("Reset Password", parameters, dialogOptions);
            
        }


        public void Cancel()
        {
            mudDialog.Cancel();
        }
    }
}
