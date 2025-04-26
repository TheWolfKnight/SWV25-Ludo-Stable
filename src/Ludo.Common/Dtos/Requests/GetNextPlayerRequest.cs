
namespace Ludo.Common.Dtos.Requests;

public record GetNextPlayerRequestDto
{
  public required byte CurrentPlayer { get; init; }

  public required PlayerDto[] Players { get; init; }
}
