using System.Net;
using System.Net.Http.Json;
using Ludo.Blazor.Models;

namespace Ludo.Blazor.Features.Game;

public class DieService
{
  private readonly HttpClient _httpClient;

  public DieService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<int> RollDieAsync(GameState state, int currentRoll)
  {
    string url = $"v1/roll?DieType={state.Die.GetType().Name}&CurrentRoll={currentRoll}";

    var response = await _httpClient.GetAsync(url);
    
    if (response.StatusCode is not HttpStatusCode.OK)
      throw new InvalidOperationException($"Failed to get new roll. Status code: {response.StatusCode}");

    int? roll = await response.Content.ReadFromJsonAsync<int>();
    
    if(roll is null)
      throw new InvalidOperationException("Failed to get roll.");
    
    return roll.Value;
  } 
}
