using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class ResetPasswordBase : ComponentBase
    {
        [Inject] IAuthenticationService AuthenticationService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        public bool isLoading { get; set; }

        public PasswordResetRequestDTO passwordResetRequest { get; set; } = new PasswordResetRequestDTO();
        public Response response { get; set; }

        public async Task ResetPasswordDialogOnClick()
        {
            // Validation for empty fields
            if (string.IsNullOrEmpty(passwordResetRequest.Email) || string.IsNullOrEmpty(passwordResetRequest.Token) || string.IsNullOrEmpty(passwordResetRequest.NewPassword))
            {
                Snackbar.Add("All Fields Are Required", Severity.Warning);
                return;
            }

            try
            {
                isLoading = true;
                // Call the Authentication service to reset the password
                bool isResetSuccessful = await AuthenticationService.ResetPasswordAsync(passwordResetRequest);

                // Display success or failure messages based on response
                if (isResetSuccessful)
                {
                    Snackbar.Add("Password reset successful!", Severity.Success);
                }
                else
                {
                    Snackbar.Add("Invalid token or email. Please try again", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
