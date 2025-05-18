using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using System.Net;
using System.Net.Http.Json;

namespace Ludo.Blazor.Features.Game;

public class PlayerService
{
  private readonly HttpClient _httpClient;
  private readonly DieFactory _dieFactory;

  public PlayerService(HttpClient httpClient, DieFactory dieFactory)
  {
    _httpClient = httpClient;
    _dieFactory = dieFactory;
  }

  public async Task<GameState> MakeMoveAsync(GameState state, int pieceTilePosition, int roll)
  {
    const string url = "v1/move";

    MakeMoveRequestDto request = new()
    {
      Game = state.ToDto(),
      PiecePosition = pieceTilePosition,
      Roll = roll
    };

    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(url, request);

    if (response.StatusCode is not HttpStatusCode.OK)
    {
      //TODO: this
    }

    GameDto? game = await response.Content.ReadFromJsonAsync<GameDto>();
    if (game is null)
      throw new InvalidOperationException("Could not deserialize response to GameDto object");

    GameState result = GameState.FromDto(game, _dieFactory);
    return result;
  }
}
