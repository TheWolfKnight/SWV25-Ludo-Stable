using System;
using System.Threading.Tasks;
using Ludo.Common.Dtos;
using Ludo.Application.Interfaces;
using Ludo.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Ludo.Api.Controllers;

[EnableCors]
[ApiController]
[Route("api/[controller]")]
public class DieController : ControllerBase
{
  private readonly IDieService _dieService;

  public DieController(IDieService dieService)
  {
    _dieService = dieService;
  }

  [HttpPost("v1/roll")]
  public async Task<ActionResult<DieDto>> RollDieAsync([FromQuery] DieDto dto)
  {
    int roll = await Task.Run(() => _dieService.RollDie(dto));
    DieDto result = new()
    {
      DieType = dto.DieType,
      CurrentRoll = roll
    };

    return Ok(result);
  }

  [HttpGet("v1/peek")]
  public async Task<ActionResult<DieDto>> PeekDieAsync([FromQuery] DieDto dto)
  {
    int roll = await Task.Run(() => _dieService.PeekDieRoll(dto));
    DieDto result = new()
    {
      DieType = dto.DieType,
      CurrentRoll = roll
    };

    return Ok(result);
  }
}
