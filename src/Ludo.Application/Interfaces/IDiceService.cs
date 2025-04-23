using Ludo.Common.Models;

namespace Ludo.Application.Interfaces;

public interface IDiceService
{
    int RollDie(GameOrchestrator orchestrator);
}