using System.Collections.Generic;

namespace Ludo.Common.Dtos;

public record PlayerDto
{
  public required byte PlayerNr { get; init; }
  public required bool InPlay { get; init; }

  public required int GoalTile { get; init; }
  public required IEnumerable<int> HomeTiles { get; init; }
  public required IEnumerable<int> PieceLocation  { get; init; }
}
