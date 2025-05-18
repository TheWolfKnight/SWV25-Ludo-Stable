namespace Ludo.Blazor.Features.Game;

public class MoveService
{
  private readonly HttpClient _httpClient;

  public MoveService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }
}
