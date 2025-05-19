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

  public PlayerService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }
}
