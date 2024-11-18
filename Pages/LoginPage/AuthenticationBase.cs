using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using CapstoneIdeaGenerator.Client.Components;
using MudBlazor;
using CapstoneIdeaGenerator.Client.Models.DTO;

namespace CapstoneIdeaGenerator.Client.Pages.LoginPage
{
    public class AuthenticationBase : ComponentBase
    {
        private readonly DialogOptions dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true, NoHeader = true };
        [Inject] public IAuthenticationService AuthService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ILocalStorageService LocalStorage { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public CustomAuthStateProvider CustomAuthStateProvider { get; set; }

        public ForgotPasswordDialogBase forgotPasswordBase;
        public LoginRequestDTO login = new LoginRequestDTO();
        public string responseMessage = string.Empty;
        public bool isSuccess;
        public bool isLoading = false;

        public async Task LoginOnClick()
        {
            isLoading = true;

            var token = await AuthService.LoginAsync(login);

            if (!string.IsNullOrEmpty(token))
            {
                Console.WriteLine($"Token Recieve: {token}");

                await LocalStorage.SetItemAsync("authToken", token);
                await LocalStorage.SetItemAsync("isAdminLoggedIn", true);

                CustomAuthStateProvider.MarkUserAsAuthenticated(token);
                responseMessage = "Login Successfully";
                NavigationManager.NavigateTo("/capstone");
            }
            else
            {
                responseMessage = "Invalid username or password.";
                isSuccess = false;
            }

            isLoading = false;
        }

        public async Task OpenForgotPasswordDialog()
        {
            var parameter = new DialogParameters<ForgotPasswordDialog>();
            var dialogOptions = new DialogOptions { CloseButton = true, FullWidth = true };

            var dialog = DialogService.Show<ForgotPasswordDialog>("Forgot Password", parameter, dialogOptions);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                Snackbar.Add("Reset Token Has Been Sent To Your Email", Severity.Success, options => options.VisibleStateDuration = 4000);
            }
            else
            {
                Snackbar.Add("Operation canceled.", Severity.Warning, options => options.VisibleStateDuration = 3000);
            }
        }

    }
}
