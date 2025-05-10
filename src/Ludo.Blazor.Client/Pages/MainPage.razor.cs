using Ludo.Blazor.Client.Features.Game;
using Ludo.Blazor.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Client.Pages
{
  public partial class MainPage : ComponentBase
  {

    private GameState? _gameState;

    private readonly GameService _service;

    public MainPage(GameService service)
    {
      _service = service;
    }

    public async Task NewGameAsync()
    {
      _gameState = await _service.GetNewGameAsync();

      StateHasChanged();
    }
  }
}
