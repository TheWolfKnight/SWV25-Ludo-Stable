using Microsoft.AspNetCore.Mvc;

namespace Ludo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiceController : ControllerBase
{
  [HttpGet("/v1/RollDie")]
  public Task<ActionResult<int>> RollDie()
  {
    throw new NotImplementedException();
  }
}