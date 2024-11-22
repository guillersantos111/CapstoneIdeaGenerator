using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CapstoneIdeaGenerator.Client.Pages.AboutPage
{
    public class AboutBase : ComponentBase
    {
        public TimelinePosition position { get; set; } = TimelinePosition.Alternate;
        public TimelineOrientation orientation { get; set; } = TimelineOrientation.Horizontal;
        public bool reverse { get; set; }
        public bool isLoading = false;

        public void OnOrientationChange(TimelineOrientation value)
        {
            orientation = value;
            switch (value)
            {
                case TimelineOrientation.Vertical:
                    if (position is TimelinePosition.Top or TimelinePosition.Bottom)
                        position = TimelinePosition.Start;
                    break;
                case TimelineOrientation.Horizontal:
                    if (position is TimelinePosition.Start or TimelinePosition.Left or TimelinePosition.Right or TimelinePosition.End)
                        position = TimelinePosition.Top;
                    break;
            }
        }

        public bool IsSwitchDisabled()
        {
            if (position == TimelinePosition.Alternate)
                return false;
            else
                reverse = false;
            return true;
        }
    }
}
