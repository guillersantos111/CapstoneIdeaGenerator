using CapstoneIdeaGenerator.Client.Models.DTOs;
using CapstoneIdeaGenerator.Client.Services.Contracts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Components
{
    public class EditCapstoneDialogBase : ComponentBase
    {
        [Parameter] public CapstonesDTO capstones { get; set; } = new CapstonesDTO();

        [CascadingParameter] private MudDialogInstance mudDialog { get; set; }
        [Inject] private ICapstoneService capstoneService { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }

        public async Task Save()
        {
            if (capstones != null)
            {
                if (capstones.CapstoneId == 0)
                {
                    await capstoneService.AddCapstone(capstones);
                }
                else
                {
                    await capstoneService.UpdateCapstone(capstones.CapstoneId, capstones);
                }
            }
            else
            {
                snackbar.Add("Error: Capstone Not Found", Severity.Error);
            }

            mudDialog.Close(DialogResult.Ok(true));
        }

        public void Cancel()
        {
            mudDialog.Cancel();
        }
    }
}