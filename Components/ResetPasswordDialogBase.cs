using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using CapstoneIdeaGenerator.Client.Utilities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class ResetPasswordDialogBase : ComponentBase
    {
        [Parameter] public string Token { get; set; }
        [Parameter] public AdminPasswordResetDTO adminPasswordReset { get; set; } = new AdminPasswordResetDTO();
        [CascadingParameter] MudDialogInstance mudDialog { get; set; }
        [Inject] IAdminService adminService { get; set; }
        [Inject] ISnackbar snackbar { get; set; }
        public ShowPasswordUtil showPassword { get; set; } = new ShowPasswordUtil();
        public Response response { get; set; } = new Response();

        public async Task Submit()
        {
            try
            {
                if (string.IsNullOrEmpty(Token))
                {
                    response.ErrorMessage = "Reset Token is required.";
                    return;
                }

                if (adminPasswordReset.NewPassword != adminPasswordReset.ConfirmPassword)
                {
                    response.ErrorMessage = "Passwords do not match.";
                    return;
                }

                var resetpassword = new AdminPasswordResetDTO
                {
                    Token = adminPasswordReset.Token,
                    NewPassword = adminPasswordReset.NewPassword,
                    ConfirmPassword = adminPasswordReset.ConfirmPassword
                };

                var result = await adminService.ResetPassword(resetpassword);

                if (result == null)
                {
                    response.ErrorMessage = string.Empty;
                }
                else
                {
                    snackbar.Add("Password Reset Successfully", Severity.Success);
                    mudDialog.Close(DialogResult.Ok(true));
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
        }


        public void ShowPassword()
        {
            showPassword.Toggle();
        }


        public void Cancel()
        {
            mudDialog.Cancel();
        }
    }
}
