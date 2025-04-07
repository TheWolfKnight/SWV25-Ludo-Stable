using Ludo.Common.Models.Player;
using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Enums;

namespace Ludo.Common.Models.Tiles;

public class GoalTile: TileBase, IGoalTile
{
  public override required byte? PlayerNr { get; init; }
  public required DriveWayTile PreviusTile { get; set; }

  public override void MovePiece(Piece piece, int amount)
  {
    if (piece.Owner.PlayerNr != this.PlayerNr)
    {
      piece.MoveToHome();
      return;
    }

    (bool moveAccepted, TileBase targetTile) = InternalMakeMove(piece, amount);

    if (!moveAccepted)
      return;

    base.Pieces.Remove(piece);
    targetTile.TakePiece(piece);
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    if (piece.Owner.PlayerNr != this.PlayerNr)
      return false;
    return InternalMakeMove(piece, amount).MoveAccepted;
  }

  internal override (bool MoveAccepted, TileBase TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    if (amount is 0)
      return (true, this);

    //TODO: this should be asked to the teacher:
    //      Should the pieces in the goal be counted as on the board for rule 1
    //      "you cannot jump over own pieces"
    // For now, I will assume it does
    bool containsOwnPieces = base.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr);
    if (containsOwnPieces)
      return (false, this);

    return PreviusTile.DriveWayMakeMove(piece, amount - 1, false);
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.InGoal;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
}
