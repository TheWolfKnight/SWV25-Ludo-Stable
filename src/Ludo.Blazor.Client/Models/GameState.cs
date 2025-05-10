using Ludo.Common.Dtos;
using Ludo.Common.Models;
using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;

namespace Ludo.Blazor.Client.Models;

public class GameState
{
  public required Board Board { get; set; }
  public required DieBase Die { get; set; }
  public required List<Player> Players { get; set; }
  public required Player CurrentPlayer { get; set; }
 
  public static GameState FromDto(GameDto dto)
  {
    return new GameState { };
  }
}