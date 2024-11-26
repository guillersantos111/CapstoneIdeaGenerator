using Blazored.LocalStorage;
using CapstoneIdeaGenerator.Client.Components;
using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Net.Http.Json;

namespace CapstoneIdeaGenerator.Client.Pages.LoginPage
{
    public class AuthenticationBase : ComponentBase
    {
        private readonly DialogOptions dialogOptions = new DialogOptions { MaxWidth = MaxWidth.Large, FullWidth = true, NoHeader = true };

        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public IAuthenticationService AuthService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public ILocalStorageService LocalStorage { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }
        [Inject] public IDialogService DialogService { get; set; }
        [Inject] public CustomAuthStateProvider CustomAuthStateProvider { get; set; }
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public ForgotPasswordDialogBase forgotPasswordBase;
        public LoginRequestDTO login = new LoginRequestDTO();
        public AdminDTO admin = new AdminDTO();
        public string responseMessage = string.Empty;
        public bool isSuccess;
        public bool isLoading = false;

        public async Task LoginOnClick()
        {
            isLoading = true;

            var result = await httpClient.PostAsJsonAsync("/api/Authentication/login", login);
            var token = await result.Content.ReadAsStringAsync();
            Console.WriteLine(token);

            if (result.IsSuccessStatusCode)
            {
                isLoading = false;
                await LocalStorage.SetItemAsync("token", token);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("/dashboard");
            }
            else
            {
                isLoading = false;
                responseMessage = "Login failed. Please check your credentials and try again.";
                Snackbar.Add(responseMessage, Severity.Error);
            }
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
