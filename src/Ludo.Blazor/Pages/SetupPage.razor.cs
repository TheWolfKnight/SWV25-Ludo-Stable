using Ludo.Blazor.Features.Game;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using Microsoft.AspNetCore.Components;

namespace Ludo.Blazor.Pages;

public partial class SetupPage : ComponentBase
{
    [Inject]
    public required GameService Service { get; set; }
    
    [Inject]
    public required GameState GameState { get; set; }
    
    private List<PlayerSetup> _players = [];
    
    private bool _isGameCreated = false;

    private async Task CreateGameAsync()
    {
        GameState = await Service.GetNewGameAsync(_players.Count);
    }

    private async Task RollDiceAsync(int playerNr)
    {
        //
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