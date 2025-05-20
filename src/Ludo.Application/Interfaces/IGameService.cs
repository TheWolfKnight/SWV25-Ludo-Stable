using Ludo.Common.Dtos;
using Ludo.Common.Dtos.Requests;
using Ludo.Common.Models;

namespace Ludo.Application.Interfaces;

public interface IGameService
{
  Task<GameOrchestrator> GenerateGameAsync();
  GameDto NextPlayer(GetNextPlayerRequestDto request);
}