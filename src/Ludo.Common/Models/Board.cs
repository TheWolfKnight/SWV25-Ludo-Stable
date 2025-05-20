using System;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Models;

public class Board
{
  public required int X { get; set; }
  public required int Y { get; set; }

  public required TileBase[] Tiles { get; init; }
}
