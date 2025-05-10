
using System.Net;
using System.Net.Http.Json;
using Ludo.Blazor.Client.Models;
using Ludo.Common.Dtos;

namespace Ludo.Blazor.Client.Features.Game;

public class GameService
{
  private readonly HttpClient _httpClient;

  public GameService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<GameState> GetNewGameAsync()
  { 
    const string url = "api/game/v1/new";

    var response = await _httpClient.GetAsync(url);
    if (response.StatusCode is not HttpStatusCode.OK)
    {
      throw new InvalidOperationException($"Failed to get new game. Status code: {response.StatusCode}");
    }

    GameDto? gameDto = await response.Content.ReadFromJsonAsync<GameDto>();

    if(gameDto is null)
    {
      throw new InvalidOperationException("Failed to deserialize game data.");
    }

    return GameState.FromDto(gameDto);
  }

}