
namespace Ludo.Common.Models.Player;

public class Player
{
  public required byte PlayerNr { get; init; }
  public required bool InPlay { get; set; } = true;

  public int RollsThisTurn { get; set; }
  public bool PieceOnBoardAtTurnStart { get; set; }

  public required Piece[] Pieces { get; init; }
  public required Home Home { get; init; }
}
