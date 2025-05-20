
using Ludo.Common.Dtos;
using System.Text.Json;

namespace Ludo.Common.Models.Tiles;

public class FillerTile: TileBase
{
  internal static FillerTile FromDto(TileDto tileDto)
  {
    int? playerNr = ((JsonElement?)tileDto.Data[nameof(PlayerNr)])?.Deserialize<int?>();

    FillerTile result = new()
    {
      PlayerNr = (byte?)playerNr,
      IndexInBoard = ((JsonElement?)tileDto.Data[nameof(IndexInBoard)])?.Deserialize<int>() ?? throw new NotImplementedException("Tiles must know their index on the board")
    };

    return result;
  }
}
