using Ludo.Common.Enums;
using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class DriveWayTile: TileBase, IGoalTile
{
  public required override byte? PlayerNr { get; init; }

  public required IGoalTile NextTile { get; set; }
  public required DriveWayTile? PreviusTile { get; set; }

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
    return DriveWayMakeMove(piece, amount, true);
  }

  public (bool MoveAccepted, TileBase TargetTile) DriveWayMakeMove(Piece piece, int amount, bool goForward)
  {
    if (amount is 0)
      return (true, this);

    bool containsOwnPiece = base.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr);
    if (containsOwnPiece)
      return (false, this);

    IGoalTile? nextTile = goForward
      ? NextTile
      : PreviusTile;

    if (nextTile is null)
    {
      nextTile = NextTile;
      goForward = true;
    }

    (bool, TileBase) result;
    if (nextTile is DriveWayTile)
      result = ((DriveWayTile)nextTile).DriveWayMakeMove(piece, amount - 1, goForward);
    else
      result = ((TileBase)nextTile).InternalMakeMove(piece, amount - 1);

    return result;
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
}
