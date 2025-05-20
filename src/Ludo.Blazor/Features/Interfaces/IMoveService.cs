using Ludo.Blazor.Models;

namespace Ludo.Blazor.Features.Interfaces;

public interface IMoveService
{
  Task<GameState> MakeMoveAsync(GameState state, int pieceTilePosition, int roll);
  Task<bool> PeekMoveAsync(GameState state, int pieceTilePosition, int roll);
}
