using System.Collections.Generic;
using Ludo.Common.Enums;

namespace Ludo.Common.Dtos;

public record TileDto
{
  public required TileTypes Type { get; init; }

  public required Dictionary<string, object> Data { get; init; } = [];
}
