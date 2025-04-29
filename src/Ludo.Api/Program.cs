using Ludo.Application.Factories;
using Ludo.Application.Services;
using Ludo.Common.Models.Dice;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<DieBase, DieD6>();
builder.Services.AddScoped<DieFactory>();

builder.Services.AddScoped<DieService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<MoveService>();
builder.Services.AddScoped<BoardGenerationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

