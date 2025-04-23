using System;
using System.Threading.Tasks;
using Ludo.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
  [HttpPut("/v1/next-player")]
  public Task<ActionResult<byte>> GetNextPlayerAsync([FromBody] GameDto request)
  {
    throw new NotImplementedException();
  }
}
