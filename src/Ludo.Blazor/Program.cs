using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ludo.Blazor;
using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Features.Game;
using Ludo.Common.Models.Dice;
using Ludo.Blazor.Models;
using Ludo.Blazor.Features.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IDieService, DieService>();
builder.Services.AddScoped<IMoveService, MoveService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Services.AddHttpClient("game", client => {
  client.BaseAddress = new Uri("https://localhost:7046/api/Game/");
});
builder.Services.AddHttpClient("move", client => {
  client.BaseAddress = new Uri("https://localhost:7046/api/Move/");
});
builder.Services.AddHttpClient("die", client => {
  client.BaseAddress = new Uri("https://localhost:7046/api/Die/");
});
builder.Services.AddHttpClient("player", client => {
  client.BaseAddress = new Uri("https://localhost:7046/api/Player/");
});

builder.Services.AddSingleton<PlayerColorMap>();

builder.Services.AddTransient<DieBase, DieD6>();
builder.Services.AddScoped<DieFactory>();
builder.Services.AddScoped<IGameStateService, GameStateService>();

var app = builder.Build();

await app.RunAsync();
