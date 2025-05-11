
using Ludo.Common.Dtos;

namespace Ludo.Common.Models.Tiles;

public class FillerTile: TileBase
{
  internal static FillerTile FromDto(TileDto tileDto)
  {
    FillerTile result = new()
    {
      PlayerNr = (byte?)tileDto.Data[nameof(PlayerNr)],
      IndexInBoard = tileDto.Data[nameof(IndexInBoard)] as int? ?? throw new NotImplementedException("Tiles must know their index on the board")
    };

    return result;
  }
}
