using Ludo.Common.Enums;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class StandardTile : TileBase
{
  public required TileBase NextTile { get; set; }

  public override void MovePiece(Piece piece, int amount)
  {
    (bool moveAccepted, TileBase targetTile) = InternalMakeMove(piece, amount);
    if (!moveAccepted)
      return;

    base.Pieces.Remove(piece);

    int cntOpponentPieces = base.Pieces.Count(inner => inner.Owner.PlayerNr != piece.Owner.PlayerNr);
    if (cntOpponentPieces > 1)
    {
      piece.MoveToHome();
      return;
    }
    else if (cntOpponentPieces == 1)
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

    bool containsOwnPieces = base.Pieces.Any(inner => inner.Owner.PlayerNr == piece.Owner.PlayerNr);
    if (amount is not 0 && containsOwnPieces)
      return (false, this);

    return NextTile.InternalMakeMove(piece, amount - 1);
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.OnBoard;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
}
