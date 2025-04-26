using Ludo.Application.Helpers;
using Ludo.Common.Dtos.Requests;
using Ludo.Common.Models;

namespace Ludo.Application.Services;

public class GameService
{
  public GameOrchestrator GenerateGame(int amountOfPlayers)
  {
    throw new NotImplementedException();
  }

  public byte NextPlayer(GetNextPlayerRequestDto request)
  {
    GameOrchestrator go = new GameOrchestrator
    {
      Board = null!,
      Die = null!,
      CurrentPlayer = request.CurrentPlayer,
      Players = request.Players.ToPlayerNrModels(),
    };

    go.NextPlayer();

    return go.CurrentPlayer;
  }
}
