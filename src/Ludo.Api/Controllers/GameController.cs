using Ludo.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController
{
  [HttpGet("/v1/Game")]
  public Task<ActionResult<GameDto>> GetGameControl()
  {
    throw new NotImplementedException();
  }

  [HttpPost("/v1/Game")]
  public Task<ActionResult<GameDto>> PostGameControl(GameDto game)
  {
    throw new NotImplementedException();
  }
}