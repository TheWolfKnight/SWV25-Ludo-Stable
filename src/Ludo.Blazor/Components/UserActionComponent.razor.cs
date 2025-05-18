using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Components
{
  public partial class UserActionComponent: ComponentBase
  {
    [Parameter]
    public string? Style { get; set; }

    [Parameter, EditorRequired]
    public required bool EnableSkipTurnButton { get; set; }

    [Parameter, EditorRequired]
    public required EventCallback OnRollDieClicked { get; set; }
    [Parameter, EditorRequired]
    public required EventCallback OnSkipTurnClicked { get; set; }

    private async Task RollDieButtonClicked()
    {
      if (OnRollDieClicked is EventCallback callback)
        await callback.InvokeAsync();
    }

    private async Task SkipTurnButtonClicked()
    {
      if (OnSkipTurnClicked is EventCallback callback)
        await callback.InvokeAsync();
    }
  }
}
