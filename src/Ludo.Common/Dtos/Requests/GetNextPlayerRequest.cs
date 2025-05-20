
namespace Ludo.Common.Dtos.Requests;

public record GetNextPlayerRequestDto
{
  public required GameDto Game { get; set; }
  public required bool MadeMove { get; set; }
}
