
namespace Ludo.Blazor.Features.Helpers;

public static class DependencyInjectionHelpers
{
  public static IServiceCollection AddHttpClient<TService>(this IServiceCollection @this, string uri)
  where TService: class
  {
    @this.AddHttpClient<TService>(client =>
    {
      client.BaseAddress = new Uri(uri);
    });

    return @this;
  }
}
