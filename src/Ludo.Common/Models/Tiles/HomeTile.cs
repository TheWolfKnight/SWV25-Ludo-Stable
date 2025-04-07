using Ludo.Common.Models.Player;
using Ludo.Common.Enums;

namespace Ludo.Common.Models.Tiles;

public class HomeTile: TileBase
{
  public override required byte? PlayerNr { get; init; }
  public required TileBase NextTile { get; set; }

  public void SendPieceHome(Piece piece)
  {
    TakePiece(piece);
  }

  public override void MovePiece(Piece piece, int amount)
  {
    (bool moveAccepted, TileBase targetTile) = InternalMakeMove(piece, amount);

    if (!moveAccepted)
      return;

    int opponentCount = NextTile.Pieces.Count(inner => inner.Owner.PlayerNr != piece.Owner.PlayerNr);
    if (opponentCount > 1)
      return;
    else if (opponentCount == 1)
    {
      Piece opp = targetTile.Pieces.First();
      opp.MoveToHome();
    }

    base.Pieces.Remove(piece);
    targetTile.TakePiece(piece);
  }

  public override bool PeekMove(Piece piece, int amount) 
  {
    return InternalMakeMove(piece, amount).MoveAccepted;
  }

  internal override (bool MoveAccepted, TileBase TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    (bool, TileBase) result = (true, NextTile);
    if (amount is not 6)
      result = (false, this);

    return result;
  }

  internal override void TakePiece(Piece piece)
  {
    piece.PieceState = PieceState.Home;
    piece.CurrentTile = this;
    base.Pieces.Add(piece);
  }
}
