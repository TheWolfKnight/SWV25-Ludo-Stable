using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
  [HttpGet("/v1/NextPlayer")]
  public Task<ActionResult<byte>> GetNextPlayerAsync()
  {
    throw new NotImplementedException();
  }
}
