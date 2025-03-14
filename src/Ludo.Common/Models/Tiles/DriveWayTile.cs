using System;
using Ludo.Common.Interfaces.Tiles;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class DriveWayTile: TileBase
{
  public required IGoalTile NextTile { get; init; }
  public required DriveWayTile? PreviusTile { get; init; }

  public override bool MovePiece(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }

  public bool DriveWayMove(Piece piece, int amount, bool forward)
  {
    throw new NotImplementedException();
  }
}
