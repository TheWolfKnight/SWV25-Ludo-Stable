using Ludo.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Components;

public partial class PlayerCard : ComponentBase
{
    [Parameter, EditorRequired]
    public required PlayerSetup Player { get; set; }
    
    [Parameter, EditorRequired]
    public required bool IsGameCreated { get; set; }

    [Parameter]
    public EventCallback<int> OnRollClicked { get; set; }

    private async Task RollClickedAsync()
    {
        await OnRollClicked.InvokeAsync(Player.PlayerNr);
    }
}