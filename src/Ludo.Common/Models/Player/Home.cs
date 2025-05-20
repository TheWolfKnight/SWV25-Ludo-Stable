using Ludo.Common.Models.Tiles;

namespace Ludo.Common.Models.Player;

public class Home
{
  public required HomeTile[] HomeTiles { get; init; }
  public required Player Owner { get; set; }

  public virtual HomeTile GetFirstAvailableHomeTile()
  {
    HomeTile? availableTile = this.HomeTiles.FirstOrDefault(tile => !tile.Pieces.Any());

    if (availableTile is null)
      throw new InvalidOperationException("Could not find valid HomeTile for home move, please check home tile to piece ratio");

    return availableTile;
  }
}
