using System.Net;
using System.Net.Http.Json;
using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Features.Interfaces;
using Ludo.Blazor.Models;
using Ludo.Common.Dtos;
using Ludo.Common.Models.Dice;

namespace Ludo.Blazor.Features.Game;

public class DieService: IDieService
{
  private readonly HttpClient _httpClient;
  private readonly DieFactory _dieFactory;

  public DieService(IHttpClientFactory clientFactory, DieFactory dieFactory)
  {
    _httpClient = clientFactory.CreateClient("die");
    _dieFactory = dieFactory;
  }

  public async Task<DieBase> RollDieAsync(GameState state)
  {
    string url = $"v1/roll?DieType={state.Die.GetType().FullName}&CurrentRoll={state.Die.PeekRoll()}";

    var response = await _httpClient.PostAsync(url, null);
    
    if (response.StatusCode is not HttpStatusCode.OK)
      throw new InvalidOperationException($"Failed to get new roll. Status code: {response.StatusCode}");

    DieDto? result = await response.Content.ReadFromJsonAsync<DieDto>();
    
    if(result is null)
      throw new InvalidOperationException("Failed to get roll.");

    DieBase die = _dieFactory.GetRolledDie(result);
    return die;
  } 
}
