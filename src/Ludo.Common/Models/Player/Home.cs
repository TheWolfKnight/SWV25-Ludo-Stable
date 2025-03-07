using System.Collections.Generic;
using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Models.Player;

public class Home
{
  public required HomeTile[] HomeTiles { get; init; }
  public required Player Owner { get; init; }
  public required List<Piece> Pieces { get; set; }
}
