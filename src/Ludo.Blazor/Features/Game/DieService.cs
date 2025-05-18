namespace Ludo.Blazor.Features.Game;

public class DieService
{
  private readonly HttpClient _httpClient;

  public DieService(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }
}
