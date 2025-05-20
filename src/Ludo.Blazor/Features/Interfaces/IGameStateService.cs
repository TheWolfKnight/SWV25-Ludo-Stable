
using Ludo.Blazor.Models;

namespace Ludo.Blazor.Features.Interfaces;

public interface IGameStateService
{
  void SetGameState(GameState gameState);
  GameState? GetGameState();
}