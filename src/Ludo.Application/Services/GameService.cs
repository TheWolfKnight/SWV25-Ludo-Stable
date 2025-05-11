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

  public async Task<GameOrchestrator> GenerateGame(int amountOfPlayers)
  {
    const string path = "./GamePresets/4v4Game.json";

    FileStream fs = File.OpenRead(path);
    GameDto? dto = await JsonSerializer.DeserializeAsync<GameDto>(fs);

    if (dto is null)
      throw new InvalidOperationException($"Could not deserialize from path \"{path}\"");

    if (!IsValidGameDto(dto))
      throw new InvalidOperationException($"The loaded game from path \"{path}\" is invalid");

    return _service.GenerateBoard(dto);
  }

  public byte NextPlayer(GetNextPlayerRequestDto request)
  {
    GameOrchestrator go = new GameOrchestrator
    {
      Board = null!,
      Die = null!,
      CurrentPlayer = request.CurrentPlayer,
      Players = request.Players.ToPlayerNrModels(),
    };

    go.NextPlayer();

    return go.CurrentPlayer;
  }

  private static bool IsValidGameDto(GameDto dto)
  {
    bool validTileset = IsTileSetupValid(dto.Tiles);
    bool validPlayer = IsValidPlayerSetup(dto.Players, dto.Tiles);

    return true;
  }

  private static bool IsValidPlayerSetup(PlayerDto[] players, TileDto[] tiles)
  {
    foreach (PlayerDto player in players)
    {
      int startTile = (int)tiles[player.HomeTiles.First()].Data[nameof(HomeTile.NextTile)]!;
      bool allHomeTilesPointToSameStart = player.HomeTiles
        .Select(i => tiles[i])
        .Where(tile => tile.Type is TileTypes.Home)
        .All(tile => (int)tile.Data[nameof(HomeTile)]! == startTile);

      if (!allHomeTilesPointToSameStart)
        return false;

      HashSet<int> visitedTiles = new();

      TileDto? currentTile = tiles[player.HomeTiles.First()];

      while (currentTile is not null)
      {
        
      }
    }

    return true;
  }

  private static bool IsTileSetupValid(TileDto[] tiles)
  {
    //NOTE: check if all movement tiles point to other movement tiles
    foreach (TileDto tile in tiles)
    {
      if (tile.Type is TileTypes.Filler)
        continue;

      bool valid = true;

      if (tile.Type is not TileTypes.Goal)
      {
        int index = (int)tile.Data["NextTile"]!;
        valid = tiles[index].Type is not TileTypes.Filler;
      }
      if (tile.Type is TileTypes.Filter)
      {
        int index = (int)tile.Data[nameof(FilterTile.FilterdTile)]!;
        valid = tiles[index].Type is not TileTypes.Filler;
      }
      if (tile.Type is (TileTypes.DriveWay or TileTypes.Goal))
      {
        int index = (int)tile.Data["PreviousTile"]!;
        valid = tiles[index].Type is not TileTypes.Filler;
      }

      //NOTE: this could be warned better with FluentResults if we care later
      if (!valid)
        return false;
    }

    return true;
  }
}
