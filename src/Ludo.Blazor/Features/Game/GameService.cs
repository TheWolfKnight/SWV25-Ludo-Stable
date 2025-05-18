using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;

namespace Ludo.Blazor.Features.Game;

public class GameService
{
  private readonly HttpClient _httpClient;
  private readonly DieFactory _dieFactory;

  public GameService(HttpClient httpClient, DieFactory dieFactory)
  {
    _httpClient = httpClient;
    _dieFactory = dieFactory;
  }

  public async Task<GameState> GetNewGameAsync(int playerAmount)
  { 
    string url = $"api/Game/v1/new?playerCount={playerAmount}";

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

    return GameState.FromDto(gameDto, _dieFactory);
  }
}