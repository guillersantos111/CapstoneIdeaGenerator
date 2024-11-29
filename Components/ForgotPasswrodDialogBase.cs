using CapstoneIdeaGenerator.Client.Models.DTO;
using CapstoneIdeaGenerator.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class ForgotPasswrodDialogBase : ComponentBase
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public AdminForgotPasswordDTO adminForgot { get; set; } = new AdminForgotPasswordDTO();
        [Inject] IAuthenticationService authenticationService { get; set; }

        public Response Response { get; set; }
    }
}
