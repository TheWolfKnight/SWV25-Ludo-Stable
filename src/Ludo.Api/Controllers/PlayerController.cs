using System;
using System.Threading.Tasks;
using Ludo.Common.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
  [HttpPut("/v1/next")]
  public Task<ActionResult<byte>> GetNextPlayerAsync([FromBody] GameDto request)
  {
    throw new NotImplementedException();
  }
}
