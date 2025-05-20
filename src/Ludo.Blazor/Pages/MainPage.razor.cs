using Ludo.Blazor.Features.Game;
using Ludo.Blazor.Models;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;
using Ludo.Common.Models.Tiles;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Pages
{
  public partial class MainPage : ComponentBase
  {
    [Inject]
    public required GameService GameService { get; set; }
    [Inject]
    public required MoveService MoveService { get; set; }
    [Inject]
    public required DieService DieService { get; set; }
    [Inject]
    public required PlayerService PlayerService { get; set; }
    [Inject]
    public required PlayerColorMap ColorMap { get; set; }
    [Inject]
    public required GameStateService GameStateService { get; set; }

    private GameState? _gameState;
    private bool _availableMoves = false;

    protected override async Task OnInitializedAsync()
    {
      ColorMap.MakeDefaultColorMap();

      if (GameStateService.GetGameState() is not null)
      {
        _gameState = GameStateService.GetGameState();
        return;
      } 
      
      await NewGameAsync(4);
    }

    private async Task NewGameAsync(int playerAmount)
    {
      _gameState = await GameService.GetNewGameAsync(playerAmount);

      StateHasChanged();
    }

    private async Task HasAvaliableMovesAsync()
    {
      await Task.CompletedTask;
      _availableMoves = true;
      StateHasChanged();
    }

    private async Task RollDieAsync()
    {
      if (_gameState is null)
        throw new InvalidOperationException("A game must be active to roll a die");

      DieBase die = await DieService.RollDieAsync(_gameState);
      _gameState.Die = die;

      _gameState.CurrentPlayer.RollsThisTurn++;

      StateHasChanged();
    }

    private async Task RequestNextPlayerAsync()
    {
      if (_gameState is null)
        throw new InvalidOperationException("A game must be active to get next player");

      byte nextPlayerNr = await PlayerService.GetNextPlayerAsync(_gameState);
      _gameState.SetNextPlayerByNr(nextPlayerNr);

      StateHasChanged();
    }

    private async Task MakeMoveAsync(Piece piece)
    {
      if (_gameState is null)
        throw new InvalidOperationException("A game must be active to make a move");

      GameState newState = await MoveService.MakeMoveAsync(
        _gameState,
        piece.CurrentTile.IndexInBoard,
        _gameState.Die.PeekRoll()
      );

      _gameState = newState;

      StateHasChanged();
    }
  }
}
