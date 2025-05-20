using Ludo.Common.Models.Dice;
using Ludo.Common.Enums;
using Ludo.Common.Models.Tiles;
using Ludo.Common.Models.Player;

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
    if (
      (!madeMove && (!player.PieceOnBoardAtTurnStart && player.RollsThisTurn < 3)) ||
      Die.PeekRoll() == 6
    )
      return;

    Player.Player[] availablePlayers = Players
      .Where(player => player.InPlay)
      .ToArray();

    byte nextPlayer = CurrentPlayer;
    int checkPlayers = 0;
    while (checkPlayers++ < availablePlayers.Length)
    {
      int playerIndex = ((CurrentPlayer + 1) % availablePlayers.Length);
      if (availablePlayers[nextPlayer].InPlay)
      {
        nextPlayer = availablePlayers[playerIndex].PlayerNr;
        break;
      }
    }

    if (checkPlayers == availablePlayers.Length)
      throw new InvalidOperationException("Ingen gyldigt spiller fundet");

    CurrentPlayer = nextPlayer;

    player = Players.First(player => player.PlayerNr == nextPlayer);
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
