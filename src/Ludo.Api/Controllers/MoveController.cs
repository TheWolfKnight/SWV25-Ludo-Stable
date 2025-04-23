using Ludo.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoveController
{
  [HttpPost("/v1/Move")]
  public Task<ActionResult<GameDto>> MovePiece(int pieceToMove, int amountToMove)
  {
    throw new NotImplementedException();
  }

  [HttpGet("/v1/Peek")]
  public Task<ActionResult<bool>> PeekMove(int pieceToMove, int amountToPeek)
  {
    throw new NotImplementedException();
  }

  [HttpGet("/v1/Valid")]
  public Task<ActionResult<bool>> CheckValid(int pieceToMove, int amountToCheck)
  {
    throw new NotImplementedException();
  }
}
