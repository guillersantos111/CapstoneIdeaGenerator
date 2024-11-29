using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class ForgotPasswordDialogBase : ComponentBase
    {
        [Parameter] public AdminForgotPasswordDTO AdminForgot { get; set; } = new AdminForgotPasswordDTO();
        public readonly DialogOptions dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, NoHeader = true };
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Inject] IAuthenticationService AuthenticationService { get; set; }
        [Inject] IClipboardService ClipboardService { get; set; }
        [Inject] IDialogService DialogService { get; set; }

        public string ResetToken { get; set; }
        public string ErrorMessage { get; set; }

        public async Task Submit()
        {
            try
            {
                var response = await AuthenticationService.ForgotPassword(AdminForgot);

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
            await ClipboardService.CopyText(ResetToken);

            var parameters = new DialogParameters()
            {
                { "Token", ResetToken },
                { "adminPasswordReset", new AdminPasswordResetDTO() }
            };

            var dialog = DialogService.Show<ResetPasswordDialog>("Reset Password", parameters, dialogOptions);
            
        }


        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
