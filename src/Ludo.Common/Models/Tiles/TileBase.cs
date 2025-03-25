using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public abstract class TileBase
{
  public byte? PlayerNr { get; init; }
  public required (int X, int Y) Location { get; set; }
  public required List<Piece> Pieces { get; set; }

  public abstract void MovePiece(Piece piece, int amount);
  public abstract bool PeekMove(Piece piece, int amount); 
}
