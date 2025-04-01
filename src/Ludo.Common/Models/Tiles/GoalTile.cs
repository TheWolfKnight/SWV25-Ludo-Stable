using Ludo.Common.Models.Player;
using Ludo.Common.Interfaces.Tiles;

namespace Ludo.Common.Models.Tiles;

public class GoalTile: TileBase, IGoalTile
{
  public required DriveWayTile PreviusTile { get; init; }

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

