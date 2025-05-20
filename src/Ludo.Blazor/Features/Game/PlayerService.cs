using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Ludo.Blazor.Features.Interfaces;
using System.Net.Http.Json;
using System.Net;

namespace Ludo.Blazor.Features.Game;

public class PlayerService: IPlayerService
{
  private readonly HttpClient _httpClient;
  private readonly DieFactory _dieFactory;

  public PlayerService(IHttpClientFactory clientFactory, DieFactory dieFactory)
  {
    _httpClient = clientFactory.CreateClient("player");
    _dieFactory = dieFactory;
  }

  public async Task<GameState> GetNextPlayerAsync(GameState state, bool madeMove)
  {
    const string url = "v1/next";

    Console.WriteLine(state.CurrentPlayer.RollsThisTurn);

    GetNextPlayerRequestDto request = new()
    {
      Game = state.ToDto(),
      MadeMove = madeMove
    };

    HttpResponseMessage response = await _httpClient.PutAsJsonAsync(url, request);

    if (response.StatusCode is not HttpStatusCode.OK)
      throw new InvalidOperationException($"Could not get valid response from game server, response: {response.StatusCode}");

    GameDto? game = await response.Content.ReadFromJsonAsync<GameDto>();
    if (game is null)
      throw new InvalidOperationException($"Invalid response from game server, got invalid GameDto back");

    GameState result = GameState.FromDto(game, _dieFactory);

    return result;
  }
}
