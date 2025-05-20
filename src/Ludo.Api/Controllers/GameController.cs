using System;
using System.Threading.Tasks;
using Ludo.Application.Interfaces;
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
  private readonly IGameService _gameService;
  private readonly IBoardGenerationService _boardGenerationService;

  public GameController(IGameService gameService, IBoardGenerationService boardGenerationService)
  {
    _gameService = gameService;
    _boardGenerationService = boardGenerationService;
  }

  [HttpGet("v1/new")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<GameDto>> GetNewGameAsync()
  {
    try
    {
      var response = await _gameService.GenerateGameAsync();

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
