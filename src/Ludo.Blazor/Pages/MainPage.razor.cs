using Ludo.Blazor.Features.Game;
using Ludo.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Pages
{
  public partial class MainPage : ComponentBase
  {

    private GameState? _gameState;

    [Inject]
    public required GameService Service { get; set; }

    protected override async Task OnInitializedAsync()
    {
      await NewGameAsync(4);
    }

    public async Task NewGameAsync(int playerAmount)
    {
      _gameState = await Service.GetNewGameAsync(playerAmount);

      StateHasChanged();
    }
  }
}
