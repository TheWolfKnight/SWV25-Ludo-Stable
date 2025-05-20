using Ludo.Blazor.Features.Game;
using Ludo.Blazor.Models;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;
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

    private bool _canRoll = true;
    private bool _canMove = false;

    private bool _showFailedMoveMsg = false;

    protected override async Task OnInitializedAsync()
    {
      ColorMap.MakeDefaultColorMap();

      if (GameStateService.GetGameState() is not null)
      {
        _gameState = GameStateService.GetGameState();
      }
      else
      {
        await NewGameAsync();
      }
      StateHasChanged();
    }

    private async Task NewGameAsync()
    {
      _gameState = await GameService.GetNewGameAsync();
    }

    private async Task HasAvaliableMovesAsync()
    {
      if (_gameState is null)
        throw new InvalidOperationException("A game must be active to check available moves");

      bool canMovePiece = await GameService.PlayerHasValidMoveAsync(_gameState);

      bool rollingForGettingPieceOut = !_gameState.CurrentPlayer.PieceOnBoardAtTurnStart &&
        _gameState.CurrentPlayer.RollsThisTurn < 3;
      bool rollingForMove = _gameState.CurrentPlayer.PieceOnBoardAtTurnStart &&
        _gameState.CurrentPlayer.RollsThisTurn < 1;

      _canMove = canMovePiece;
      _canRoll = (rollingForGettingPieceOut || rollingForMove) && !canMovePiece;
      StateHasChanged();
    }

    private async Task RollDieAsync()
    {
      if (_gameState is null)
        throw new InvalidOperationException("A game must be active to roll a die");

      if (!_canRoll || _canMove)
        return;

      DieBase die = await DieService.RollDieAsync(_gameState);
      _gameState.Die = die;

      _gameState.CurrentPlayer.RollsThisTurn++;
      await HasAvaliableMovesAsync();

      StateHasChanged();
    }

    private async Task RequestNextPlayerAsync(bool madeMove = false)
    {
      if (_gameState is null)
        throw new InvalidOperationException("A game must be active to get next player");

      GameState newState = await PlayerService.GetNextPlayerAsync(_gameState, madeMove);
      _gameState = newState;
      _canMove = false;
      _canRoll = true;

      StateHasChanged();
    }

    private async Task MakeMoveAsync(Piece piece)
    {
      if (_gameState is null)
        throw new InvalidOperationException("A game must be active to make a move");

      if (!_canMove)
        return;

      GameState newState = await MoveService.MakeMoveAsync(
        _gameState,
        piece.CurrentTile.IndexInBoard,
        _gameState.Die.PeekRoll()
      );

      //NOTE: if the piece moved between dtos, the player piece must have moved
      IEnumerable<int> oldPieceLocation = _gameState.CurrentPlayer.Pieces.Select(piece => piece.CurrentTile.IndexInBoard);
      IEnumerable<int> newPieceLocation = newState.CurrentPlayer.Pieces.Select(piece => piece.CurrentTile.IndexInBoard);

      IEnumerable<(int old, int @new)> locations = oldPieceLocation.Zip(newPieceLocation);
      bool playersHasMadeMove = locations.Any(set => set.old != set.@new);

      _gameState = newState;

      if (playersHasMadeMove)
      {
        await RequestNextPlayerAsync(true);
        _showFailedMoveMsg = false;
      }
      else
        _showFailedMoveMsg = true;

      StateHasChanged();
    }
  }
}
