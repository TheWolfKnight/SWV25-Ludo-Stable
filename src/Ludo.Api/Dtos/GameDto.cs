namespace Ludo.Api.Dtos;

public class GameDto
{
  public required int Version { get; init; }
  public required int X { get; init; }
  public required int Y { get; init; }
  
  public required PlayerDto CurrentPlayer { get; init; }
  
  public required TileDto[] Tiles { get; init; }
  public required PlayerDto[] Players { get; init; }
}