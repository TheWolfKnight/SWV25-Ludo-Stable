using System;
using System.Threading.Tasks;
using Ludo.Application.Services;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[EnableCors]
[ApiController]
[Route("api/[controller]")]
public class MoveController: ControllerBase
{
  private readonly MoveService _service;

  public MoveController(MoveService service)
  {
    _service = service;
  }

  [HttpPost("v1/move")]
  public async Task<ActionResult<GameDto>> MovePieceAsync([FromBody] MakeMoveRequestDto request)
  {
    GameDto result;
    try
    {
      result = await Task.Run(() => _service.MovePiece(request));
    }
    catch (Exception e)
    {
      return BadRequest("Could not find a piece on the designated tile");
    }

    return Ok(result);
  }

  [HttpPut("v1/valid")]
  public async Task<ActionResult<bool>> CheckValidAsync([FromBody] CheckValidMoveRequestDto request)
  {
    bool result = await Task.Run(() => _service.PeekPieceMove(request));
    return Ok(result);
  }
}
