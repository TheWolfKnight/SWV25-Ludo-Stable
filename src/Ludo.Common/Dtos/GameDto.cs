namespace Ludo.Common.Dtos;

public record GameDto
{
  public required int Version { get; init; }
  public required int X { get; init; }
  public required int Y { get; init; }

  public required byte CurrentPlayer { get; init; }
  
  public required DieDto Die { get; init; }
  
  public required TileDto[] Tiles { get; init; }
  public required PlayerDto[] Players { get; init; }
}
