using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Pages.AdminPages.LoginPage;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace CapstoneIdeaGenerator.Client.Shared
{
    public partial class AdminLayout
    {
        [Inject] private IAdminService  adminService { get; set; }
        [Inject] private IActivityLogsService activityLogsService { get; set; }
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private CustomAuthStateProvider CustomAuthStateProvider { get; set; }

        public AdminLoginDTO logout = new AdminLoginDTO();
        public ActivityLogsDTO adminLogs { get; set; } = new ActivityLogsDTO();

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
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var admin = authenticationState.User;

            if (admin.Identity.IsAuthenticated)
            {
                logout.Email = admin.Identity.Name;


                var fetchadmin = await adminService.GetAdminByEmail(logout.Email);

                if (fetchadmin != null)
                {
                    adminLogs = new ActivityLogsDTO
                    {
                        AdminId = fetchadmin.AdminId,
                        Email = fetchadmin.Email,
                        Name = fetchadmin.Name,
                        Action = "Logged Out",
                        Details = "Admin Successfully Logged Out",
                        Timestamp = DateTime.UtcNow
                    };

                    await activityLogsService.RecordLogsActivity(adminLogs);
                    await CustomAuthStateProvider.ClearAdminSession();
                    NavigationManager.NavigateTo("/authentication");
                }
                else
                {
                    Console.WriteLine($"Admin with email {logout.Email} not found");
                }
            }
            else
            {
                Console.WriteLine("Admin Is Not Authenticated");
            }
        }
    }
}
