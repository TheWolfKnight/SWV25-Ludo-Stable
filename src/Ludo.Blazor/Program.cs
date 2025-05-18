using System.Linq;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ludo.Blazor;
using Ludo.Blazor.Features.Factory;
using Ludo.Blazor.Features.Game;
using Ludo.Common.Models.Dice;
using Ludo.Blazor.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<GameService>(client =>
{
  client.BaseAddress = new Uri("https://localhost:7046/");
});

builder.Services.AddSingleton<PlayerColorMap>();

builder.Services.AddTransient<DieBase, DieD6>();
builder.Services.AddScoped<DieFactory>();

var app = builder.Build();

await app.RunAsync();
