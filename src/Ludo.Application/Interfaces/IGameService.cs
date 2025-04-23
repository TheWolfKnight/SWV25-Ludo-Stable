using Ludo.Common.Models;

namespace Ludo.Application.Interfaces;

public interface IGameService
{
    GameOrchestrator GenerateGame(int amountOfPlayers);
    GameOrchestrator NextPlayer(GameOrchestrator gameOrchestrator);
}