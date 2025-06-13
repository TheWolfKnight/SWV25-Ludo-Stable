using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using System.Net;
using System.Net.Http.Json;
using Ludo.Blazor.Features.Interfaces;

namespace Ludo.Blazor.Features.Game;

public class GameService : IGameService
{
  private readonly HttpClient _httpClient;
  private readonly DieFactory _dieFactory;

  public GameService(IHttpClientFactory clientFactory, DieFactory dieFactory)
  {
    _httpClient = clientFactory.CreateClient("game");
    _dieFactory = dieFactory;
  }

  public async Task<GameState> GetNewGameAsync()
  { 
    string url = $"v1/new";

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

  public async Task<bool> PlayerHasValidMoveAsync(GameState state)
  {
    const string url = "v1/any-valid-move";

    AnyValidMoveRequeset request = new()
    {
      Game = state.ToDto(),
      Roll = state.Die.PeekRoll(),
    };

    HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, request);

    if (response.StatusCode is not HttpStatusCode.OK)
      throw new InvalidOperationException($"Could not get valid response from game server, response: {response.StatusCode}");

    return await response.Content.ReadAsStringAsync() is "true";
  }

  public async Task<byte[]> DetirminStaringPlayerAsync(int[] playerRolls)
  {
    string url = $"v1/detirmin-starting-player?playerRolls={string.Join("&playerRolls=", playerRolls)}";

    HttpResponseMessage response = await _httpClient.GetAsync(url);

    if (response.StatusCode is not HttpStatusCode.OK)
      throw new InvalidOperationException($"Could not get valid response from game server, response {response.StatusCode}");

    byte[]? highestRollers = await response.Content.ReadFromJsonAsync<byte[]>();
    if (highestRollers is null)
      throw new InvalidCastException($"Could not deserialize response to byte[], response: \"{await response.Content.ReadAsStringAsync()}\"");

    return highestRollers;
  }
}