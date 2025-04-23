using Ludo.Common.Enums;

namespace Ludo.Api.Dtos;

public class TileDto
{
  public required TileTypes Type { get; init; }

  public required Dictionary<string, object> Data { get; init; } = [];
}
