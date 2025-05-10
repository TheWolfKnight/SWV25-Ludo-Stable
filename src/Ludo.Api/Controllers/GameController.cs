using System;
using System.Threading.Tasks;
using Ludo.Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController: ControllerBase
{
  [HttpGet("/v1/new")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public Task<ActionResult<GameDto>> GetNewGameAsync([FromQuery] int playerCount)
  {
    throw new NotImplementedException();
  }
}
