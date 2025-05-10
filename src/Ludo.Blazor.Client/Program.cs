using Ludo.Blazor.Client.Features.Game;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient<GameService>(client =>
{
  client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BASE_ADDRESS") ?? "http://localhost:5176");
});


await builder.Build().RunAsync();


