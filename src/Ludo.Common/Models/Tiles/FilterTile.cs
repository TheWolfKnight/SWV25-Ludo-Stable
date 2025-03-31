using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class FilterTile: TileBase
{
  public required TileBase NextTile { get; set; }
  public required DriveWayTile FilterdTile { get; set; }

  public override void MovePiece(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }
}
