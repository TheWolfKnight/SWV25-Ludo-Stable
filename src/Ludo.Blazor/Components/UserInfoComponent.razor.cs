using Ludo.Blazor.Models;
using Ludo.Common.Models.Player;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Components
{
  public partial class UserInfoComponent: ComponentBase
  {
    [Inject]
    public required PlayerColorMap ColorMap { get; set; }

    [Parameter, EditorRequired]
    public required GameState GameState { get; set; }

    [Parameter, EditorRequired]
    public required bool CanMove { get; set; }
    [Parameter, EditorRequired]
    public required bool CanRoll { get; set; }
    [Parameter, EditorRequired]
    public required bool MadeInvalidMove { get; set; }

    [Parameter]
    public string? Style { get; set; }

    public string GetCurrentPlayerColorName()
    {
      return ColorMap.GetPlayerColor(GameState.CurrentPlayer.PlayerNr);
    }

    public string GetInfoTextColor()
    {
      return "info-text-" + GetCurrentPlayerColorName();
    }
  }
}
