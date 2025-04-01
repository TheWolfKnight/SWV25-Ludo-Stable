using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class StandardTile : TileBase
{
  public required TileBase NextTile { get; init; }

  public override void MovePiece(Piece piece, int amount)
  {
    (bool moveAccepted, TileBase targetTile) = InternalMakeMove(piece, amount);
    if (!moveAccepted)
      return;

    int cntOpponentPieces = base.Pieces.Count(inner => inner.Owner.PlayerNr != piece.Owner.PlayerNr);
    if (cntOpponentPieces > 1)
    {
      piece.MoveToHome();
      return;
    }

    if (cntOpponentPieces == 1)
    {
      Piece opp = base.Pieces.First();
      opp.MoveToHome();
    }

    targetTile.Pieces.Add(piece);
    piece.CurrentTile = targetTile;
    base.Pieces.Remove(piece);
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    return InternalMakeMove(piece, amount).MoveAccepted;
  }

  internal override (bool MoveAccepted, TileBase TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    if (amount is 0)
      return (true, this);

    bool containsOwnPieces = base.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr);
    if (amount is not 0 && containsOwnPieces)
      return (false, this);

    return NextTile.InternalMakeMove(piece, amount - 1);
  }
}
