using System.Threading.Tasks;
using Ludo.Application.Services;
using Ludo.Common.Dtos.Requests;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[DisableCors]
[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
  private readonly GameService _service;

  public PlayerController(GameService service)
  {
    _service = service;
  }

  [HttpPut("v1/next")]
  public async  Task<ActionResult<byte>> GetNextPlayerAsync([FromBody] GetNextPlayerRequestDto request)
  {
    byte nextPlayer = await Task.Run(() => _service.NextPlayer(request));

    return Ok(nextPlayer);
  }
}
