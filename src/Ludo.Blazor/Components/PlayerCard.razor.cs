using Ludo.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Components;

public partial class PlayerCard : ComponentBase
{
    [Parameter, EditorRequired]
    public required PlayerSetup Player { get; set; }
    
    [Parameter, EditorRequired]
    public required bool IsGameCreated { get; set; }
    
    [Parameter, EditorRequired]
    public required int? Roll { get; set; }

    [Parameter]
    public EventCallback<int> OnRollClicked { get; set; }
    
    [Parameter]
    public EventCallback<(byte, string)> OnColorClicked { get; set; }
    
    [Inject]
    public required PlayerColorMap ColorMap { get; set; }

    private async Task RollClickedAsync()
    {
        await OnRollClicked.InvokeAsync(Player.PlayerNr);
    }

    private string GetPlayerColorStyle()
    {
        string playerColor = ColorMap.GetPlayerColor((byte)Player.PlayerNr);
        
        return $"{playerColor}-background";
    }
}