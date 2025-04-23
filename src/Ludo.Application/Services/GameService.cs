using Ludo.Application.Interfaces;
using Ludo.Common.Models;

namespace Ludo.Application.Services;

public class GameService : IGameService
{
    public GameOrchestrator GenerateGame(int amountOfPlayers)
    {
        throw new NotImplementedException();
    }

    public GameOrchestrator NextPlayer(GameOrchestrator gameOrchestrator)
    {
        gameOrchestrator.NextPlayer();
        
        return gameOrchestrator;
    }
}