using System.Threading.Tasks;
using Ludo.Application.Services;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Ludo.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[EnableCors]
[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
  private readonly IGameService _service;

  public PlayerController(IGameService service)
  {
    _service = service;
  }

  [HttpPut("v1/next")]
  public async  Task<ActionResult<GameDto>> GetNextPlayerAsync([FromBody] GetNextPlayerRequestDto request)
  {
    GameDto dto = await Task.Run(() => _service.NextPlayer(request));

    return Ok(dto);
  }
}
