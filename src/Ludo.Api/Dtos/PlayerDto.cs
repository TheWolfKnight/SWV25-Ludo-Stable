namespace Ludo.Api.Dtos;

public class PlayerDto
{
  public required byte PlayerNo { get; init; }

  public required IEnumerable<int> HomeTiles { get; init; }
  public required IEnumerable<int> PieceLocation  { get; init; }
}
