using Blazored.LocalStorage;
using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class EditLoginBase : ComponentBase
    {
        public LoginRequestDTO login = new LoginRequestDTO();
        public string errorMessage;
        public bool isLoading = false;

        [Inject] public IAuthenticationService AuthenticationService { get; set; }
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }

        public async Task HandleLogin()
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                errorMessage = "Both fields are required.";
                return;
            }

            try
            {
                var response = await AuthenticationService.LoginAsync(login);

                if (response != null)
                {
                    await LocalStorageService.SetItemAsync("authToken", response);
                    NavigationManager.NavigateTo("/generator");
                }
                else
                {
                    errorMessage = "Invalid credentials, please try again.";
                    Snackbar.Add(errorMessage, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
                Snackbar.Add(errorMessage, Severity.Error);
            }
        }
    }
}
