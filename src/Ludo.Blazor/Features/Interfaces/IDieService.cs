using Ludo.Blazor.Models;
using Ludo.Common.Models.Dice;

namespace Ludo.Blazor.Features.Interfaces;

public interface IDieService
{
  Task<DieBase> RollDieAsync(GameState state);

}
