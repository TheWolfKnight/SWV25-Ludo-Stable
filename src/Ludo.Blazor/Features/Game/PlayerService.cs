using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Ludo.Common.Models.Player;
using System.Net;
using System.Net.Http.Json;

namespace Ludo.Blazor.Features.Game;

public class PlayerService
{
  private readonly HttpClient _httpClient;

  public PlayerService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<byte> GetNextPlayerAsync(GameState state)
  {
    const string url = "v1/next";

    GetNextPlayerRequestDto request = new()
    {
      Game = state.ToDto()
    };

    HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, request);

    if (response.StatusCode is not HttpStatusCode.OK)
      throw new InvalidOperationException($"Could not get valid response from game server, response: {response.StatusCode}");

    string? nextPlayer = await response.Content.ReadAsStringAsync();
    if (nextPlayer is not string str || !byte.TryParse(str, out byte playerNr))
      throw new InvalidOperationException($"Invalid response from game server, response was: \"{nextPlayer}\"");

    return playerNr;
  }
}
