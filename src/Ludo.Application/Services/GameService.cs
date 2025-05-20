using System;
using System.Reflection;
using System.Text.Json;
using Ludo.Application.Helpers;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Ludo.Common.Enums;
using Ludo.Common.Models;
using Ludo.Common.Models.Tiles;

namespace Ludo.Application.Services;

public class GameService
{
  private readonly BoardGenerationService _service;

  public GameService(BoardGenerationService service)
  {
    _service = service;
  }

  public async Task<GameOrchestrator> GenerateGameAsync(int amountOfPlayers)
  {
    string asmPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? ".";
    string path = asmPath + "\\GamePresets\\4v4Game.json";

    FileStream fs = File.OpenRead(path);
    GameDto? dto = await JsonSerializer.DeserializeAsync<GameDto>(fs);

    if (dto is null)
      throw new InvalidOperationException($"Could not deserialize from path \"{path}\"");

    return _service.GenerateBoard(dto);
  }

  public byte NextPlayer(GetNextPlayerRequestDto request)
  {
    GameOrchestrator go = _service.GenerateBoard(request.Game);
    go.NextPlayer();

    return go.CurrentPlayer;
  }
}
