using Ludo.Blazor.Models;

namespace Ludo.Blazor.Features.Interfaces;

public interface IGameService
{
  Task<GameState> GetNewGameAsync();
  Task<bool> PlayerHasValidMoveAsync(GameState state);
  Task<byte[]> DetirminStaringPlayerAsync(int[] playerRolls);
}
