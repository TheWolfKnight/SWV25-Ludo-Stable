using System;
using System.Threading.Tasks;
using Ludo.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController
{
  [HttpGet("/v1/game")]
  public Task<ActionResult<GameDto>> GetNewGameAsync(int playerCount)
  {
    throw new NotImplementedException();
  }
}
