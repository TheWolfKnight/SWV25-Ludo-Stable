using Ludo.Application.Interfaces;
using Ludo.Common.Models;

namespace Ludo.Application.Services;

public class DiceService : IDiceService
{
    public int RollDie(GameOrchestrator orchestrator)
    {
        return orchestrator.Die.Roll();
    }
}