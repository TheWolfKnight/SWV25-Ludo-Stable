using System;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Models;

public class Board
{
  public required TileBase[] Tiles { get; init; }
  public void LoadNewBoard()
  {
    throw new NotImplementedException();
  }
}
