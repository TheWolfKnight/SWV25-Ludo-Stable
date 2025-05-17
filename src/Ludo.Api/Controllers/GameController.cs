using System;
using System.Threading.Tasks;
using Ludo.Application.Services;
using Ludo.Common.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Ludo.Api.Controllers;

[EnableCors]
[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
  private readonly GameService _gameService;
  private readonly BoardGenerationService _boardGenerationService;


  public GameController(GameService gameService, BoardGenerationService boardGenerationService)
  {
    _gameService = gameService;
    _boardGenerationService = boardGenerationService;
  }

  [HttpGet("v1/new")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GameDto>> GetNewGameAsync([FromQuery] int playerCount)
  {
    if (playerCount < 2 || playerCount > 4) return BadRequest("Invalid player count.");
    try
    {
      var response = await _gameService.GenerateGame(playerCount);

      var boardDto = _boardGenerationService.CompressBoardToDto(response);

      return Ok(boardDto);
    }
    catch (Exception e)
    {
      return Problem(title: "Error", detail: e.Message);
    }
  }
}
