using System;
using System.Reflection;
using System.Text.Json;
using Ludo.Application.Factories;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Ludo.Common.Models;
using Ludo.Application.Interfaces;

namespace Ludo.Application.Services;

public class GameService : IGameService
{
  private readonly IBoardGenerationService _service;
  private readonly DieFactory _dieFactory;

  public GameService(IBoardGenerationService service, DieFactory dieFactory)
  {
    _service = service;
    _dieFactory = dieFactory;
  }

  public async Task<GameOrchestrator> GenerateGameAsync()
  {
    string asmPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? ".";
    string path = asmPath + "/GamePresets/4v4Game.json";

    FileStream fs = File.OpenRead(path);
    GameDto? dto = await JsonSerializer.DeserializeAsync<GameDto>(fs);

    if (dto is null)
      throw new InvalidOperationException($"Could not deserialize from path \"{path}\"");

    return _service.GenerateBoard(dto);
  }

  public GameDto NextPlayer(GetNextPlayerRequestDto request)
  {
    byte oldPlayerNr = request.Game.CurrentPlayer;
    GameOrchestrator go = _service.GenerateBoard(request.Game);
    go.NextPlayer(request.MadeMove);

    if (oldPlayerNr != go.CurrentPlayer)
      go.Die = _dieFactory.GetDie(go.Die.GetType()?.FullName ?? "Unknown");

    return _service.CompressBoardToDto(go);
  }
}
