using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class DriveWayTile: TileBase, IGoalTile
{
  public required IGoalTile NextTile { get; init; }
  public required DriveWayTile? PreviusTile { get; init; }

  public override void MovePiece(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }

  internal override (bool MoveAccepted, TileBase TargetTile) InternalMakeMove(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }
}
