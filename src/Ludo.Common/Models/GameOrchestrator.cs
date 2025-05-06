using Ludo.Common.Models.Dice;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models;

public class GameOrchestrator
{
  public required byte CurrentPlayer { get; set; }
  public required DieBase Die { get; init; }
  public required Board Board { get; init; }
  public required Player.Player[] Players { get; set; }

  public void StartGame()
  {
    throw new NotImplementedException();
  }

  public void EndGame()
  {
    throw new NotImplementedException();
  }

  public void RestartGame()
  {
    throw new NotImplementedException();
  }

  public virtual void NextPlayer()
  {
    byte nextPlayer = CurrentPlayer;
    int checkPlayers = 0;
    while (checkPlayers++ < Players.Length)
      nextPlayer = (byte)((CurrentPlayer + 1) % Players.Length);

    CurrentPlayer = nextPlayer;
  }

  public bool IsValidMove(Piece piece)
  {
    throw new NotImplementedException();
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
