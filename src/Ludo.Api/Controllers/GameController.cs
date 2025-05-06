using System;
using System.Threading.Tasks;
using Ludo.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController: ControllerBase
{
  [HttpGet("/v1/new")]
  public Task<ActionResult<GameDto>> GetNewGameAsync([FromQuery] int playerCount)
  {
    throw new NotImplementedException();
  }
}
