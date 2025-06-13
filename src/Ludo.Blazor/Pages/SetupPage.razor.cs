using Ludo.Blazor.Features.Game;
using Ludo.Blazor.Features.Interfaces;
using Ludo.Blazor.Models;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Pages;

public partial class SetupPage : ComponentBase
{
  [Inject]
  public required IGameService GameService { get; set; }

  [Inject]
  public required IDieService DieService { get; set; }

  [Inject]
  public required PlayerColorMap ColorMap { get; set; }

  [Inject]
  public required NavigationManager NavigationManager { get; set; }

  [Inject]
  public required IGameStateService GameStateService { get; set; }

  private GameState? _gameState;
  private List<PlayerSetup> _players = [];
  private Dictionary<int, int> _rolls = new();

  private bool _isGameCreated;
  private bool _isReadyToStart;

  protected override void OnInitialized()
  {
    if (!_players.Any())
      ColorMap.MakeDefaultColorMap();
  }

  private async Task CreateGameAsync()
  {
    _gameState = await GameService.GetNewGameAsync();

    _players.ForEach(x => x.CanRoll = true);
    _isGameCreated = true;

    IEnumerable<byte> playersInGame = _players.Select(item => item.PlayerNr);

    foreach (Player player in _gameState.Players)
      player.InPlay = playersInGame.Contains(player.PlayerNr);

    StateHasChanged();
  }

  private async Task RollDiceAsync(int playerNr)
  {
    if (_gameState is null)
      return;

    DieBase die = await DieService.RollDieAsync(_gameState);

    _rolls[playerNr] = die.PeekRoll();

    _players.First(x => x.PlayerNr == playerNr).CanRoll = false;

    StateHasChanged();
  }

  private async Task DetermineStartingAsync()
  {
    if (_gameState is null)
      return;

    byte[] remaining = await GameService.DetirminStaringPlayerAsync(_rolls.Values.ToArray());

    if (remaining.Length is not 1)
    {
      _rolls.Clear();

      foreach (var playerNr in remaining)
        _players.First(x => x.PlayerNr == playerNr).CanRoll = true;

      return;
    }

    _isReadyToStart = true;

    _gameState.CurrentPlayer = _gameState.Players.First(x => x.PlayerNr == remaining[0]);
  }

  private void BeginGameAsync()
  {
    if (_gameState is null)
      return;

    GameStateService.SetGameState(_gameState);

    NavigationManager.NavigateTo($"/game");
  }

  private void AddPlayer()
  {
    byte nextPlayerNumber = (byte)(_players.Count != 0 ? _players.Max(x => x.PlayerNr) + 1 : 0);

    _players.Add(new PlayerSetup()
    {
      PlayerNr = nextPlayerNumber,
      CanRoll = false
    });

    StateHasChanged();
  }
}