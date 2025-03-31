using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class StandardTile: TileBase
{
    public required TileBase NextTile { get; init; }

    public override void MovePiece(Piece piece, int amount)
    {
      throw new NotImplementedException();
    }

    public override bool PeekMove(Piece piece, int amount)
    {
      throw new NotImplementedException();
    }
}
