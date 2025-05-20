using Ludo.Common.Models.Dice;
using Ludo.Common.Enums;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Models;

public class GameOrchestrator
{
  public required byte CurrentPlayer { get; set; }
  public required DieBase Die { get; set; }
  public required Board Board { get; init; }
  public required Player.Player[] Players { get; set; }

  public virtual bool HasValidMove(int roll)
  {
    Player.Player player = Players[CurrentPlayer];

    bool anyValidMoves = player.Pieces.Any(piece =>
      piece.PieceState is not PieceState.InGoal &&
      piece.CurrentTile.PeekMove(piece, roll)
    );

    return anyValidMoves;
  }

  public virtual void NextPlayer(bool madeMove)
  {
    Player.Player player = Players[CurrentPlayer];
    if (!madeMove && (!player.PieceOnBoardAtTurnStart && player.RollsThisTurn < 3))
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

    player = Players[nextPlayer];
    player.RollsThisTurn = 0;
    player.PieceOnBoardAtTurnStart = player.Pieces
      .Any(piece => piece.CurrentTile is not (HomeTile or GoalTile));
  }

  public static byte[] DetermineStartingPlayer(int[] rolls)
  {
    int highestRoll = rolls.Max();

    List<byte> highestRollers = [];

    for (int i = 0; i < rolls.Length; i++)
      if (rolls[i] == highestRoll)
        highestRollers.Add((byte) i);

    return highestRollers.ToArray();
  }
}
