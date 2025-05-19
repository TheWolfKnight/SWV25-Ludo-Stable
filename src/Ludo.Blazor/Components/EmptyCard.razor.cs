using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Components;

public partial class EmptyCard : ComponentBase
{
    [Parameter]
    public EventCallback OnClicked { get; set; }

    private async Task ClickedAsync()
    {
        await OnClicked.InvokeAsync();
    }
}