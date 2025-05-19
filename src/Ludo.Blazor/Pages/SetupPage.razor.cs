using Ludo.Blazor.Features.Game;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Pages;

public partial class SetupPage : ComponentBase
{
    [Inject]
    public required GameService GameService { get; set; }
    
    [Inject]
    public required DieService DieService { get; set; }
    
    [Inject]
    public required PlayerColorMap ColorMap { get; set; }
    
    private GameState? _gameState;
    private List<PlayerSetup> _players = [];
    private int[] _rolls = [];
    
    private bool _isGameCreated = false;

    private async Task CreateGameAsync()
    {
        _gameState = await GameService.GetNewGameAsync(_players.Count);
        
        _players.ForEach(x => x.CanRoll = true);
        _isGameCreated = true;
        
        StateHasChanged();
    }

    private async Task RollDiceAsync(int playerNr)
    {
        // int roll = await DieService.RollDieAsync(_gameState, _gameState.Die.PeekRoll());

        Random rnd = new();
        
        _players.First(x => x.PlayerNr == playerNr).Roll = rnd.Next(1,6);
        _players.First(x => x.PlayerNr == playerNr).CanRoll = false;
        
        StateHasChanged();
    }

    private void SetPlayerColor((byte, string) playerColor)
    {
        ColorMap.AddPlayerColor(playerColor.Item1, playerColor.Item2);;
    }
    
    private void AddPlayer()
    {
        int nextPlayerNumber = _players.Any() ? _players.Max(x => x.PlayerNr) + 1 : 0;
    
        _players.Add(new PlayerSetup()
        {
            PlayerNr = nextPlayerNumber,
            CanRoll = false
        });
    
        StateHasChanged();
    }
}