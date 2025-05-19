using Ludo.Blazor.Features.Game;
using Ludo.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Pages
{
  public partial class MainPage : ComponentBase
  {
    [Inject]
    public required GameService Service { get; set; }
    
    [Inject]
    public required PlayerColorMap ColorMap { get; set; }

    private GameState? _gameState;
    private bool _availableMoves = false;

    protected override async Task OnInitializedAsync()
    {
      ColorMap.MakeDefaultColorMap();
      await NewGameAsync(4);
    }

    public async Task NewGameAsync(int playerAmount)
    {
      _gameState = await Service.GetNewGameAsync(playerAmount);

      StateHasChanged();
    }

    public async Task HasAvaliableMovesAsync()
    {
      await Task.CompletedTask;
      _availableMoves = true;
      StateHasChanged();
    }
  }
}
