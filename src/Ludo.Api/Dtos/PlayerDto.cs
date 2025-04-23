using System.Collections.Generic;

namespace Ludo.Api.Dtos;

public record PlayerDto
{
  public required byte PlayerNo { get; init; }

  public required int GoalTile { get; init; }
  public required IEnumerable<int> HomeTiles { get; init; }
  public required IEnumerable<int> PieceLocation  { get; init; }
}
