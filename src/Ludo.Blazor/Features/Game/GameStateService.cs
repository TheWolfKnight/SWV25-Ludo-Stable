using Ludo.Blazor.Features.Interfaces;
using Ludo.Blazor.Models;

namespace Ludo.Blazor.Features.Game;

public class GameStateService : IGameStateService
{
    private GameState? _gameState; 
    
    public void SetGameState(GameState gameState)
    {
        _gameState = gameState;
    }
    
    public GameState? GetGameState()
    {
        return _gameState;
    }
}