using Ludo.Blazor.Models;

namespace Ludo.Blazor.Features.Interfaces;

public interface IPlayerService
{
  Task<GameState> GetNextPlayerAsync(GameState state, bool madeMove);
}
