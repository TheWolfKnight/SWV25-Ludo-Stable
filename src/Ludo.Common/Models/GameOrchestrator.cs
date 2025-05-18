using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models;

public class GameOrchestrator
{
  public required byte CurrentPlayer { get; set; }
  public required DieBase Die { get; init; }
  public required Board Board { get; init; }
  public required Player.Player[] Players { get; set; }

  public virtual bool HasValidMove(int roll)
  {
    //TODO: this when I complete the game
    throw new NotImplementedException();
  }

  public virtual void NextPlayer()
  {
    Player.Player player = Players[CurrentPlayer];
    if (!player.PieceOnBoardAtTurnStart && player.RollsThisTurn != 3)
      return;

    byte nextPlayer = CurrentPlayer;
    int checkPlayers = 0;
    while (checkPlayers++ < Players.Length)
    {
      nextPlayer = (byte)((CurrentPlayer + 1) % Players.Length);
      if (Players[nextPlayer].InPlay)
        break;
    }

    if (checkPlayers == Players.Length)
      throw new InvalidOperationException("Ingen gyldigt spiller fundet");

    CurrentPlayer = nextPlayer;
  }

  public byte[] DetermineStartingPlayer(int[] rolls)
  {
    int highestRoll = rolls.Max();

    List<byte> highestRollers = [];

    for (int i = 0; i < rolls.Length; i++)
      if (rolls[i] == highestRoll)
        highestRollers.Add((byte) i);

    return highestRollers.ToArray();
  }
}
