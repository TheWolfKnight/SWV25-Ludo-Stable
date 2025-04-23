using System;
using System.Threading.Tasks;
using Ludo.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiceController : ControllerBase
{
  [HttpGet("/v1/roll-die")]
  public Task<ActionResult<DieDto>> RollDieAsync([FromQuery] DieDto die)
  {
    throw new NotImplementedException();
  }

  [HttpGet("/v1/peek-die")]
  public Task<ActionResult<DieDto>> PeekDieAsync([FromQuery] DieDto die)
  {
    throw new NotImplementedException();
  }
}
