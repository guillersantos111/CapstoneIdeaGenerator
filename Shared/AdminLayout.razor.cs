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
            await LocalStorage.SetItemAsync("drawerState", drawerOpen);
            drawerOpen = !drawerOpen;
        }
        public async Task Logout()
        {

            NavigationManager.NavigateTo("/authentication");
        }
    }
}
