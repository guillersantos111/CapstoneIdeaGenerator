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

        public AdminLoginDTO login = new AdminLoginDTO();
        public AccountDTO admin = new AccountDTO();
        public string responseMessage = string.Empty;
        public bool isSuccess;
        public bool isLoading = false;

        public async Task LoginOnClick()
        {
            isLoading = true;
            var response = await AuthService.LoginAsync(login);

            if (response.IsSuccess)
            {
                isLoading = false;
                NavigationManager.NavigateTo("/dashboard");
            }
            else
            {
                isLoading = false;
                responseMessage = response.Message;
                Snackbar.Add(responseMessage, Severity.Error);
            }
        }
    }
}