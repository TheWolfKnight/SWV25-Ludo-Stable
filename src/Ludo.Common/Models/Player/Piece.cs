using System;
using Ludo.Common.Enums;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Models.Player;

public class Piece
{
  public required TileBase CurrentTile;
  public required Player Owner { get; init; }
  public required PieceState PieceState { get; set; }

  public void MoveToHome()
  {
    throw new NotImplementedException();
  }
}
