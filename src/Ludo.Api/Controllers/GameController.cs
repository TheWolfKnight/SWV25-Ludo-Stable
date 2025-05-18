using System;
using System.Threading.Tasks;
using Ludo.Application.Services;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Ludo.Common.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
      var response = await _gameService.GenerateGameAsync(playerCount);

      var boardDto = _boardGenerationService.CompressBoardToDto(response);

      return Ok(boardDto);
    }
    catch (Exception e)
    {
      return Problem(title: "Error", detail: e.Message);
    }
  }

  [HttpPut("v1/any-valid-move")]
  public async Task<ActionResult<bool>> AnyValidMovesAsync([FromBody] AnyValidMoveRequeset request)
  {
    GameOrchestrator game;
    try
    {
      game = _boardGenerationService.GenerateBoard(request.Game);
    }
    catch (Exception e)
    {
      return BadRequest("Bad GameDto: "+ e.Message);
    }

    bool isValid = game.HasValidMove(request.Roll);

    return await Task.FromResult(Ok(isValid));
  }

  [HttpGet("v1/detirmin-starting-player")]
  public async Task<ActionResult<byte[]>> DetirminStartingPlayerAsync([FromQuery] int[] playerRolls)
  {
    byte[] highestRollers = GameOrchestrator.DetermineStartingPlayer(playerRolls);
    
    return Ok(await Task.FromResult(highestRollers));
  }
}
