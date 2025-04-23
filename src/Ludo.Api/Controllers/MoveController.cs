using System;
using System.Threading.Tasks;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoveController
{
  [HttpPost("/v1/move")]
  public Task<ActionResult<GameDto>> MovePieceAsync([FromBody] MakeMoveRequestDto request)
  {
    throw new NotImplementedException();
  }

  [HttpPut("/v1/valid")]
  public Task<ActionResult<bool>> CheckValidAsync([FromBody] CheckValidMoveRequestDto request)
  {
    throw new NotImplementedException();
  }
}
