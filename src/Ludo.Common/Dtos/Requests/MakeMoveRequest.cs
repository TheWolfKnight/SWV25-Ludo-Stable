
namespace Ludo.Common.Dtos.Requests;

public record MakeMoveRequestDto
{
  public required GameDto Game { get; init; }

  public required int PiecePosition { get; init; }
  public required int Roll { get; init; }
}
