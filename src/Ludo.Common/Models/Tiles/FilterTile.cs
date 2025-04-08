using Ludo.Common.Enums;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class FilterTile : TileBase
{
  public required TileBase NextTile { get; set; }
  public required DriveWayTile FilterdTile { get; set; }

  public override void MovePiece(Piece piece, int amount)
  {
    (bool moveAccepted, TileBase targetTile) = InternalMakeMove(piece, amount);
    if (!moveAccepted)
      return;

    base.Pieces.Remove(piece);

    int opponentCount = targetTile.Pieces.Count(inner => inner.Owner.PlayerNr != piece.Owner.PlayerNr);
    if (opponentCount > 1)
    {
      piece.MoveToHome();
      return;
    }
    else if (opponentCount == 1)
    {
      Piece opp = targetTile.Pieces.First();
      opp.MoveToHome();
    }

    targetTile.TakePiece(piece);
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    return InternalMakeMove(piece, amount).MoveAccepted;
  }

  internal override (bool MoveAccepted, TileBase TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    if (amount is 0)
      return (true, this);

    bool containsOwnPiece = base.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr && inner != piece);
    if (containsOwnPiece)
      return (false, this);

    bool isFilterdPlayer = base.PlayerNr == piece.Owner.PlayerNr;
    Func<Piece, int, (bool MoveAccepted, TileBase TargetTile)> method = isFilterdPlayer
      ? FilterdTile.InternalMakeMove
      : NextTile.InternalMakeMove;

    return method(piece, amount - 1);
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
}
