
namespace Ludo.Api.Dtos;

public record DieDto
{
  /// <summary>
  /// Fully qualified namespace for the die type to be used
  /// </summary>
  public required string DieType { get; init; }

  public required int? CurrentRoll { get; init; }
}
