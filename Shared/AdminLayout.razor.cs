using CapstoneIdeaGenerator.Client.Pages.LoginPage;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace CapstoneIdeaGenerator.Client.Shared
{
    public partial class AdminLayout
    {
        [Inject] private IAuthenticationService authenticationService { get; set; }
        [Inject] private CustomAuthStateProvider customAuthStateProvider { get; set; }


        public bool drawerOpen = false;

        protected override async Task OnInitializedAsync()
        {
            drawerOpen = await LocalStorage.GetItemAsync<bool>("drawerState");
        }

        public async Task ToggleDrawer()
        {
            drawerOpen = !drawerOpen;
            await LocalStorage.SetItemAsync("drawerState", drawerOpen);
        }
        public async Task Logout()
        {
            customAuthStateProvider.MarkUserAsLoggedOut();
            await LocalStorage.RemoveItemAsync("authToken");
            await LocalStorage.RemoveItemAsync("authTokenExpiration");

            NavigationManager.NavigateTo("/authentication");
        }
    }
}
