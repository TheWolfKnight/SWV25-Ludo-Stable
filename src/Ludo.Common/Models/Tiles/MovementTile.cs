
using Ludo.Common.Dtos;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public abstract class MovementTile: TileBase
{
  public required List<Piece> Pieces { get; set; }

  public abstract void MovePiece(Piece piece, int amount);
  public abstract bool PeekMove(Piece piece, int amount);
  public abstract void BindTiles(TileDto tileDto, Board board);

  internal abstract (bool MoveAccepted, MovementTile TargetTile) InternalMakeMove(Piece piece, int amount);
  internal abstract void TakePiece(Piece piece);

}
