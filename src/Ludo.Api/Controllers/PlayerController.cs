using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
  [Route("/v1/NextPlayer")]
  [HttpGet]
  public async Task<ActionResult<byte>> GetNextPlayerAsync()
  {
    throw new NotImplementedException();
  }
}
