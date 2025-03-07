using System;
using Ludo.Common.Models.Player;

namespace Ludo.Common.Models.Tiles;

public class GoalTile: TileBase
{
  public required DriveWayTile PreviusTile { get; init; }

  public override bool MovePiece(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }

  public override bool PeekMove(Piece piece, int amount)
  {
    throw new NotImplementedException();
  }
}

