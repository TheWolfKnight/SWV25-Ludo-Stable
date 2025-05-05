using System;
using System.Threading.Tasks;
using Ludo.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DieController : ControllerBase
{
  [HttpGet("/v1/roll")]
  public Task<ActionResult<DieDto>> RollDieAsync([FromQuery] DieDto die)
  {
    throw new NotImplementedException();
  }

  [HttpGet("/v1/peek")]
  public Task<ActionResult<DieDto>> PeekDieAsync([FromQuery] DieDto die)
  {
    throw new NotImplementedException();
  }
}
